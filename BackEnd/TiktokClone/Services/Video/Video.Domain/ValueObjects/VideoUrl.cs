using TiktokClone.SharedKernel.Domain;

namespace Video.Domain.ValueObjects;

/// <summary>
/// Video URL value object with validation
/// </summary>
public class VideoUrl : ValueObject
{
    public string Value { get; private set; }

    private VideoUrl(string value)
    {
        Value = value;
    }

    public static VideoUrl Create(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException("Video URL cannot be empty", nameof(url));

        // Basic URL validation
        if (!Uri.TryCreate(url, UriKind.Absolute, out var uri) || 
            (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
        {
            throw new ArgumentException("Invalid video URL format", nameof(url));
        }

        return new VideoUrl(url);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
