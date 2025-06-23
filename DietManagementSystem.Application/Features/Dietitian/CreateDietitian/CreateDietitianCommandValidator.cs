using FluentValidation;

namespace DietManagementSystem.Application.Features.Dietitian.CreateDietitian;

public class CreateDietitianCommandValidator : AbstractValidator<CreateDietitianCommand>
{
    public CreateDietitianCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(50).WithMessage("First name must not exceed 50 characters")
            .Matches(@"^[a-zA-Z\s\-']+$").WithMessage("First name contains invalid characters");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(50).WithMessage("Last name must not exceed 50 characters")
            .Matches(@"^[a-zA-Z\s\-']+$").WithMessage("Last name contains invalid characters");
    }
}
