using Identity.Domain.Events;
using Identity.Domain.ValueObjects;
using TiktokClone.SharedKernel.Domain;

namespace Identity.Domain.Entities;

/// <summary>
/// User aggregate root - handles authentication and user identity
/// </summary>
public class User : BaseEntity<Guid>, IAggregateRoot
{
    public Email Email { get; private set; }
    public string Username { get; private set; }
    public string PasswordHash { get; private set; }
    public UserRole Role { get; private set; }
    public bool IsEmailVerified { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime? LastLoginAt { get; private set; }
    public string? RefreshToken { get; private set; }
    public DateTime? RefreshTokenExpiresAt { get; private set; }

    // For EF Core
    private User() : base() { }

    private User(Guid id, Email email, string username, string passwordHash, UserRole role) : base(id)
    {
        Email = email;
        Username = username;
        PasswordHash = passwordHash;
        Role = role;
        IsEmailVerified = false;
        IsActive = true;

        AddDomainEvent(new UserRegisteredEvent(Id, email.Value, username));
    }

    public static User Create(string email, string username, string passwordHash)
    {
        var emailVO = Email.Create(email);
        return new User(Guid.NewGuid(), emailVO, username, passwordHash, UserRole.User);
    }

    public void UpdatePassword(string newPasswordHash)
    {
        PasswordHash = newPasswordHash;
        SetUpdatedAt();
        AddDomainEvent(new UserPasswordChangedEvent(Id));
    }

    public void VerifyEmail()
    {
        if (!IsEmailVerified)
        {
            IsEmailVerified = true;
            SetUpdatedAt();
            AddDomainEvent(new UserEmailVerifiedEvent(Id, Email.Value));
        }
    }

    public void UpdateRole(UserRole newRole)
    {
        if (Role != newRole)
        {
            var oldRole = Role;
            Role = newRole;
            SetUpdatedAt();
            AddDomainEvent(new UserRoleChangedEvent(Id, oldRole, newRole));
        }
    }

    public void Deactivate()
    {
        if (IsActive)
        {
            IsActive = false;
            SetUpdatedAt();
            AddDomainEvent(new UserDeactivatedEvent(Id));
        }
    }

    public void Activate()
    {
        if (!IsActive)
        {
            IsActive = true;
            SetUpdatedAt();
        }
    }

    public void RecordLogin()
    {
        LastLoginAt = DateTime.UtcNow;
        SetUpdatedAt();
        AddDomainEvent(new UserLoggedInEvent(Id, Username));
    }

    public void SetRefreshToken(string refreshToken, DateTime expiresAt)
    {
        RefreshToken = refreshToken;
        RefreshTokenExpiresAt = expiresAt;
        SetUpdatedAt();
    }

    public void ClearRefreshToken()
    {
        RefreshToken = null;
        RefreshTokenExpiresAt = null;
        SetUpdatedAt();
    }

    public bool IsRefreshTokenValid(string token)
    {
        return RefreshToken == token &&
               RefreshTokenExpiresAt.HasValue &&
               RefreshTokenExpiresAt.Value > DateTime.UtcNow;
    }
}
