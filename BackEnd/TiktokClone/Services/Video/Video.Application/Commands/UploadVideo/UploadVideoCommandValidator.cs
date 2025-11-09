using FluentValidation;

namespace Video.Application.Commands.UploadVideo;

public class UploadVideoCommandValidator : AbstractValidator<UploadVideoCommand>
{
    public UploadVideoCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters");

        RuleFor(x => x.VideoUrl)
            .NotEmpty().WithMessage("Video URL is required")
            .Must(BeAValidUrl).WithMessage("Invalid video URL format");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required");

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required");

        RuleFor(x => x.DurationSeconds)
            .GreaterThan(0).WithMessage("Duration must be greater than 0")
            .LessThanOrEqualTo(3600).WithMessage("Duration must not exceed 1 hour");

        RuleFor(x => x.FileSizeBytes)
            .GreaterThan(0).WithMessage("File size must be greater than 0")
            .LessThanOrEqualTo(500 * 1024 * 1024).WithMessage("File size must not exceed 500MB");

        RuleFor(x => x.Format)
            .NotEmpty().WithMessage("Format is required")
            .Must(BeAValidFormat).WithMessage("Invalid video format");
    }

    private bool BeAValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out var uri) &&
               (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
    }

    private bool BeAValidFormat(string format)
    {
        var validFormats = new[] { "mp4", "webm", "mov", "avi", "mkv" };
        return validFormats.Contains(format.ToLowerInvariant());
    }
}
