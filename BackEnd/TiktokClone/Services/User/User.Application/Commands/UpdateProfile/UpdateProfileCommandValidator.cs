using FluentValidation;

namespace User.Application.Commands.UpdateProfile;

public class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
{
    public UpdateProfileCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID is required");

        RuleFor(x => x.DisplayName)
            .MaximumLength(100)
            .WithMessage("Display name must not exceed 100 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.DisplayName));

        RuleFor(x => x.Bio)
            .MaximumLength(500)
            .WithMessage("Bio must not exceed 500 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Bio));
    }
}
