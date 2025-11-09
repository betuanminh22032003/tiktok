using TiktokClone.SharedKernel.Domain;

namespace User.Domain.ValueObjects;

/// <summary>
/// Avatar URL value object
/// </summary>
public class AvatarUrl : ValueObject
{
    public string Url { get; private set; }

    private AvatarUrl(string url)
    {
        Url = url;
    }

    public static AvatarUrl Create(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException("Avatar URL cannot be empty", nameof(url));

        if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
            throw new ArgumentException("Avatar URL must be a valid URL", nameof(url));

        if (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)
            throw new ArgumentException("Avatar URL must use HTTP or HTTPS", nameof(url));

        return new AvatarUrl(url);
    }

    public static AvatarUrl? CreateOrNull(string? url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return null;

        try
        {
            return Create(url);
        }
        catch
        {
            return null;
        }
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Url;
    }

    public override string ToString() => Url;
}
