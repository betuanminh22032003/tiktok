using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Security;
using Identity.Application.Dtos;
using Identity.Application.Interfaces;
using Identity.Application.Models;
using Identity.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly JwtSettings _settings;
        private readonly Identity.Application.Interfaces.IRoleService _roleService;
        // In-memory refresh token store for demo - replace with DB in production
        private readonly IDictionary<string, (string userId, DateTime expires, bool revoked)> _refreshTokens = new Dictionary<string, (string, DateTime, bool)>();

        public AuthService(IOptions<JwtSettings> options, Identity.Application.Interfaces.IRoleService roleService)
        {
            _settings = options.Value;
            _roleService = roleService;
        }

        public AuthResult Login(string username, string password)
        {
            // In a real app validate credentials against user repository and password hashing
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Invalid credentials");

            var user = new User(username, "<hashed-password-placeholder>");
            // get assigned role if present
            var role = _roleService.GetRole(user.Id.ToString());
            if (!string.IsNullOrEmpty(role)) user.SetRole(role);

            var access = GenerateAccessToken(user);
            var refresh = GenerateRefreshToken(user.Id.ToString());

            return new AuthResult
            {
                AccessToken = access.token,
                AccessTokenExpiresAt = access.expires,
                RefreshToken = refresh.token,
                RefreshTokenExpiresAt = refresh.expires,
                UserId = user.Id.ToString(),
                Username = user.Username
            };
        }

        public AuthResult Refresh(string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken)) throw new ArgumentException("Missing refresh token");
            lock (_refreshTokens)
            {
                if (!_refreshTokens.TryGetValue(refreshToken, out var entry)) throw new SecurityException("Invalid refresh token");
                if (entry.revoked || entry.expires < DateTime.UtcNow) throw new SecurityException("Refresh token invalid or expired");

                // rotate
                _refreshTokens[refreshToken] = (entry.userId, entry.expires, true);
                var user = new User(entry.userId, "");
                var role = _roleService.GetRole(entry.userId);
                if (!string.IsNullOrEmpty(role)) user.SetRole(role);
                var access = GenerateAccessToken(user);
                var newRefresh = GenerateRefreshToken(entry.userId);

                return new AuthResult
                {
                    AccessToken = access.token,
                    AccessTokenExpiresAt = access.expires,
                    RefreshToken = newRefresh.token,
                    RefreshTokenExpiresAt = newRefresh.expires,
                    UserId = entry.userId,
                    Username = user.Username
                };
            }
        }

        public void Logout(string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken)) return;
            lock (_refreshTokens)
            {
                if (_refreshTokens.TryGetValue(refreshToken, out var entry))
                {
                    _refreshTokens[refreshToken] = (entry.userId, entry.expires, true);
                }
            }
        }

        private (string token, DateTime expires) GenerateAccessToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key ?? string.Empty));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
                new Claim("role", user.Role)
            };

            var expires = DateTime.UtcNow.AddMinutes(_settings.AccessTokenExpirationMinutes);
            var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds);

            return (new JwtSecurityTokenHandler().WriteToken(token), expires);
        }

        private (string token, DateTime expires) GenerateRefreshToken(string userId)
        {
            var random = RandomNumberGenerator.GetBytes(64);
            var token = Convert.ToBase64String(random);
            var expires = DateTime.UtcNow.AddDays(_settings.RefreshTokenExpirationDays);
            lock (_refreshTokens)
            {
                _refreshTokens[token] = (userId, expires, false);
            }
            return (token, expires);
        }
    }
}
