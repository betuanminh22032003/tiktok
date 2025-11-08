using Identity.Web.Models;

namespace Identity.Web.Services
{
    public interface ITokenService
    {
        string GenerateAccessToken(UserDto user);
        RefreshToken GenerateRefreshToken(string userId);
        bool ValidateRefreshToken(string token, out RefreshToken? refreshToken);
        void RevokeRefreshToken(string token);
        RefreshToken? RotateRefreshToken(string oldToken, string userId);
    }
}
