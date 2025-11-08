using Identity.Application.Dtos;

namespace Identity.Application.Interfaces
{
    public interface IAuthService
    {
        AuthResult Login(string username, string password);
        AuthResult Refresh(string refreshToken);
        void Logout(string refreshToken);
    }
}
