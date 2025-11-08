using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Identity.Web.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Web.Services
{
    /// <summary>
    /// Minimal in-memory token service. In production persist refresh tokens in a database and support revocation and rotation safely.
    /// </summary>
    public class TokenService : ITokenService
    {
        private readonly JwtSettings _settings;
        private readonly IDictionary<string, RefreshToken> _refreshTokens = new Dictionary<string, RefreshToken>();

        public TokenService(IOptions<JwtSettings> settings)
        {
            _settings = settings.Value;
        }

        public string GenerateAccessToken(UserDto user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key ?? string.Empty));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
                new Claim("uid", user.Id)
            };

            var expires = DateTime.UtcNow.AddMinutes(_settings.AccessTokenExpirationMinutes);

            var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public RefreshToken GenerateRefreshToken(string userId)
        {
            var randomBytes = RandomNumberGenerator.GetBytes(64);
            var token = Convert.ToBase64String(randomBytes);
            var refresh = new RefreshToken
            {
                Token = token,
                UserId = userId,
                ExpiresAt = DateTime.UtcNow.AddDays(_settings.RefreshTokenExpirationDays),
                IsRevoked = false
            };

            // store - in-memory for demo
            lock (_refreshTokens)
            {
                _refreshTokens[refresh.Token] = refresh;
            }

            return refresh;
        }

        public bool ValidateRefreshToken(string token, out RefreshToken? refreshToken)
        {
            refreshToken = null;
            if (string.IsNullOrEmpty(token)) return false;

            lock (_refreshTokens)
            {
                if (_refreshTokens.TryGetValue(token, out var stored))
                {
                    if (stored.IsRevoked) return false;
                    if (stored.ExpiresAt < DateTime.UtcNow) return false;
                    refreshToken = stored;
                    return true;
                }
            }

            return false;
        }

        public void RevokeRefreshToken(string token)
        {
            if (string.IsNullOrEmpty(token)) return;
            lock (_refreshTokens)
            {
                if (_refreshTokens.TryGetValue(token, out var stored))
                {
                    stored.IsRevoked = true;
                    _refreshTokens[token] = stored;
                }
            }
        }

        public RefreshToken? RotateRefreshToken(string oldToken, string userId)
        {
            // revoke old
            if (!string.IsNullOrEmpty(oldToken)) RevokeRefreshToken(oldToken);

            // issue new
            var newToken = GenerateRefreshToken(userId);
            return newToken;
        }
    }
}
