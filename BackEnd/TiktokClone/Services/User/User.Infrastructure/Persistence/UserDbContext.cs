using Microsoft.EntityFrameworkCore;
using TiktokClone.SharedKernel.Application;
using TiktokClone.SharedKernel.Domain;
using User.Domain.Entities;

namespace User.Infrastructure.Persistence;

public class UserDbContext : DbContext, IUnitOfWork
{
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
    {
    }

    public DbSet<UserProfile> UserProfiles => Set<UserProfile>();
    public DbSet<Follow> Follows => Set<Follow>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // UserProfile Configuration
        modelBuilder.Entity<UserProfile>(entity =>
        {
            entity.ToTable("user_profiles");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedNever();

            entity.Property(e => e.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            entity.HasIndex(e => e.UserId)
                .IsUnique();

            entity.Property(e => e.Username)
                .HasColumnName("username")
                .HasMaxLength(50)
                .IsRequired();

            entity.HasIndex(e => e.Username)
                .IsUnique();

            entity.Property(e => e.DisplayName)
                .HasColumnName("display_name")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.Bio)
                .HasColumnName("bio")
                .HasMaxLength(500);

            entity.OwnsOne(e => e.Avatar, avatar =>
            {
                avatar.Property(a => a.Url)
                    .HasColumnName("avatar_url")
                    .HasMaxLength(500);
            });

            entity.Property(e => e.FollowersCount)
                .HasColumnName("followers_count")
                .HasDefaultValue(0);

            entity.Property(e => e.FollowingCount)
                .HasColumnName("following_count")
                .HasDefaultValue(0);

            entity.Property(e => e.VideoCount)
                .HasColumnName("video_count")
                .HasDefaultValue(0);

            entity.Property(e => e.TotalLikes)
                .HasColumnName("total_likes")
                .HasDefaultValue(0);

            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            entity.Property(e => e.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired();

            entity.Ignore(e => e.DomainEvents);
        });

        // Follow Configuration
        modelBuilder.Entity<Follow>(entity =>
        {
            entity.ToTable("follows");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedNever();

            entity.Property(e => e.FollowerId)
                .HasColumnName("follower_id")
                .IsRequired();

            entity.Property(e => e.FollowerUsername)
                .HasColumnName("follower_username")
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(e => e.FollowingId)
                .HasColumnName("following_id")
                .IsRequired();

            entity.Property(e => e.FollowingUsername)
                .HasColumnName("following_username")
                .HasMaxLength(50)
                .IsRequired();

            entity.HasIndex(e => new { e.FollowerId, e.FollowingId })
                .IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            entity.Ignore(e => e.DomainEvents);
        });
    }

    // IUnitOfWork Implementation
    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (Database.CurrentTransaction == null)
        {
            await Database.BeginTransactionAsync(cancellationToken);
        }
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (Database.CurrentTransaction != null)
        {
            await Database.CommitTransactionAsync(cancellationToken);
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (Database.CurrentTransaction != null)
        {
            await Database.RollbackTransactionAsync(cancellationToken);
        }
    }
}
