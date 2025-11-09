using TiktokClone.SharedKernel.Domain;

namespace Video.Domain.ValueObjects;

/// <summary>
/// Video metadata value object
/// </summary>
public class VideoMetadata : ValueObject
{
    public long FileSizeBytes { get; private set; }
    public string Format { get; private set; }
    public double FileSizeMB => FileSizeBytes / (1024.0 * 1024.0);

    private VideoMetadata(long fileSizeBytes, string format)
    {
        if (fileSizeBytes <= 0)
            throw new ArgumentException("File size must be positive", nameof(fileSizeBytes));

        if (string.IsNullOrWhiteSpace(format))
            throw new ArgumentException("Format cannot be empty", nameof(format));

        FileSizeBytes = fileSizeBytes;
        Format = format.ToLowerInvariant();
    }

    public static VideoMetadata Create(long fileSizeBytes, string format)
    {
        return new VideoMetadata(fileSizeBytes, format);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return FileSizeBytes;
        yield return Format;
    }
}
