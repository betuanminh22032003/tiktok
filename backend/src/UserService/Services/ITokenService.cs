namespace UserService.Services
{
    public interface ITokenService
    {
        string GenerateAccessToken(Guid userId, string username);
        string GenerateRefreshToken();
    }
}
