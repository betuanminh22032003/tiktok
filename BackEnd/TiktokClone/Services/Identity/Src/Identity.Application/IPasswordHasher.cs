namespace Identity.Application;

/// <summary>
/// Interface for password hashing service
/// </summary>
public interface IPasswordHasher
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string passwordHash);
}
