using Interaction.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Interaction.Infrastructure.Persistence;

public class InteractionDbContext : DbContext
{
    public InteractionDbContext(DbContextOptions<InteractionDbContext> options)
        : base(options)
    {
    }

    public DbSet<Like> Likes => Set<Like>();
    public DbSet<Comment> Comments => Set<Comment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Like entity configuration
        modelBuilder.Entity<Like>(entity =>
        {
            entity.HasKey(l => l.Id);

            entity.Property(l => l.VideoId)
                .IsRequired();

            entity.Property(l => l.UserId)
                .IsRequired();

            entity.Property(l => l.Username)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(l => l.CreatedAt)
                .IsRequired();

            // Index for finding likes by video
            entity.HasIndex(l => l.VideoId)
                .HasDatabaseName("IX_Likes_VideoId");

            // Index for finding likes by user
            entity.HasIndex(l => l.UserId)
                .HasDatabaseName("IX_Likes_UserId");

            // Unique constraint: one user can like a video only once
            entity.HasIndex(l => new { l.VideoId, l.UserId })
                .IsUnique()
                .HasDatabaseName("IX_Likes_VideoId_UserId_Unique");

            // Ignore domain events (they are handled in memory)
            entity.Ignore(l => l.DomainEvents);
        });

        // Comment entity configuration
        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(c => c.Id);

            entity.Property(c => c.VideoId)
                .IsRequired();

            entity.Property(c => c.UserId)
                .IsRequired();

            entity.Property(c => c.Username)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(c => c.Content)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(c => c.ParentCommentId)
                .IsRequired(false);

            entity.Property(c => c.CreatedAt)
                .IsRequired();

            entity.Property(c => c.UpdatedAt)
                .IsRequired(false);

            entity.Property(c => c.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            // Index for finding comments by video
            entity.HasIndex(c => c.VideoId)
                .HasDatabaseName("IX_Comments_VideoId");

            // Index for finding comments by user
            entity.HasIndex(c => c.UserId)
                .HasDatabaseName("IX_Comments_UserId");

            // Index for finding replies to a comment
            entity.HasIndex(c => c.ParentCommentId)
                .HasDatabaseName("IX_Comments_ParentCommentId");

            // Index for created date (for sorting)
            entity.HasIndex(c => c.CreatedAt)
                .HasDatabaseName("IX_Comments_CreatedAt");

            // Composite index for video + created date (optimizes feed queries)
            entity.HasIndex(c => new { c.VideoId, c.CreatedAt })
                .HasDatabaseName("IX_Comments_VideoId_CreatedAt");

            // Ignore domain events
            entity.Ignore(c => c.DomainEvents);
        });
    }
}
