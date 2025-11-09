using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Video.Domain.ValueObjects;

namespace Video.Infrastructure.Persistence;

/// <summary>
/// DbContext for Video service
/// </summary>
public class VideoDbContext : DbContext
{
    public VideoDbContext(DbContextOptions<VideoDbContext> options) : base(options)
    {
    }

    public DbSet<Domain.Entities.Video> Videos => Set<Domain.Entities.Video>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Video entity configuration
        modelBuilder.Entity<Domain.Entities.Video>(entity =>
        {
            entity.ToTable("Videos");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .ValueGeneratedNever();

            entity.Property(e => e.Title)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(e => e.Description)
                .HasMaxLength(2000);

            // VideoUrl value object
            entity.OwnsOne(e => e.VideoUrl, url =>
            {
                url.Property(u => u.Value)
                    .HasColumnName("VideoUrl")
                    .HasMaxLength(512)
                    .IsRequired();
            });

            entity.Property(e => e.ThumbnailUrl)
                .HasMaxLength(512);

            entity.Property(e => e.UserId)
                .IsRequired();

            entity.HasIndex(e => e.UserId);

            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsRequired();

            // VideoDuration value object
            entity.OwnsOne(e => e.Duration, duration =>
            {
                duration.Property(d => d.TotalSeconds)
                    .HasColumnName("DurationSeconds")
                    .IsRequired();
            });

            // Status enum as string
            entity.Property(e => e.Status)
                .HasConversion(new EnumToStringConverter<VideoStatus>())
                .HasMaxLength(50)
                .IsRequired();

            entity.HasIndex(e => e.Status);

            // Statistics
            entity.Property(e => e.ViewCount)
                .IsRequired()
                .HasDefaultValue(0);

            entity.Property(e => e.LikeCount)
                .IsRequired()
                .HasDefaultValue(0);

            entity.Property(e => e.CommentCount)
                .IsRequired()
                .HasDefaultValue(0);

            entity.Property(e => e.ShareCount)
                .IsRequired()
                .HasDefaultValue(0);

            // VideoMetadata value object
            entity.OwnsOne(e => e.Metadata, metadata =>
            {
                metadata.Property(m => m.FileSizeBytes)
                    .HasColumnName("FileSizeBytes")
                    .IsRequired();

                metadata.Property(m => m.Format)
                    .HasColumnName("Format")
                    .HasMaxLength(20)
                    .IsRequired();
            });

            entity.Property(e => e.CreatedAt)
                .IsRequired();

            entity.HasIndex(e => e.CreatedAt);

            entity.Property(e => e.UpdatedAt);

            // Ignore domain events
            entity.Ignore(e => e.DomainEvents);
        });
    }
}
