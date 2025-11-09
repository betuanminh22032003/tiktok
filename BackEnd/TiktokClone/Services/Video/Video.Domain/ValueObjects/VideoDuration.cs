using TiktokClone.SharedKernel.Domain;

namespace Video.Domain.ValueObjects;

/// <summary>
/// Video duration value object
/// </summary>
public class VideoDuration : ValueObject
{
    public int TotalSeconds { get; private set; }
    
    public int Hours => TotalSeconds / 3600;
    public int Minutes => (TotalSeconds % 3600) / 60;
    public int Seconds => TotalSeconds % 60;

    private VideoDuration(int totalSeconds)
    {
        if (totalSeconds < 0)
            throw new ArgumentException("Duration cannot be negative", nameof(totalSeconds));

        if (totalSeconds > 3600) // Max 1 hour for TikTok-like app
            throw new ArgumentException("Video duration exceeds maximum allowed length", nameof(totalSeconds));

        TotalSeconds = totalSeconds;
    }

    public static VideoDuration FromSeconds(int seconds)
    {
        return new VideoDuration(seconds);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return TotalSeconds;
    }

    public override string ToString() => $"{Hours:D2}:{Minutes:D2}:{Seconds:D2}";
}
