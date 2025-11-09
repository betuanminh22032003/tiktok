using Identity.Domain.Entities;
using Identity.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Identity.Infrastructure.Persistence;

/// <summary>
/// DbContext for Identity service
/// </summary>
public class IdentityDbContext : DbContext
{
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User entity configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .ValueGeneratedNever();

            // Email value object
            entity.OwnsOne(e => e.Email, email =>
            {
                email.Property(e => e.Value)
                    .HasColumnName("Email")
                    .HasMaxLength(256)
                    .IsRequired();

                email.HasIndex(e => e.Value)
                    .IsUnique();
            });

            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsRequired();

            entity.HasIndex(e => e.Username)
                .IsUnique();

            entity.Property(e => e.PasswordHash)
                .HasMaxLength(512)
                .IsRequired();

            // Enum as string
            entity.Property(e => e.Role)
                .HasConversion(new EnumToStringConverter<UserRole>())
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(e => e.IsEmailVerified)
                .IsRequired();

            entity.Property(e => e.IsActive)
                .IsRequired();

            entity.Property(e => e.CreatedAt)
                .IsRequired();

            entity.Property(e => e.UpdatedAt);

            entity.Property(e => e.LastLoginAt);

            entity.Property(e => e.RefreshToken)
                .HasMaxLength(512);

            entity.Property(e => e.RefreshTokenExpiresAt);

            // Ignore domain events
            entity.Ignore(e => e.DomainEvents);
        });
    }
}
