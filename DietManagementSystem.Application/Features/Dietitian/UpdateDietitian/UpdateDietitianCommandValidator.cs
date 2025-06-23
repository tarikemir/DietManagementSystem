using FluentValidation;

namespace DietManagementSystem.Application.Features.Dietitian.UpdateDietitian;

public class UpdateDietitianCommandValidator : AbstractValidator<UpdateDietitianCommand>
{
    public UpdateDietitianCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Dietitian ID is required");

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(50)
            .WithMessage("First name is required and must not exceed 50 characters");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(50)
            .WithMessage("Last name is required and must not exceed 50 characters");

        RuleFor(x => x.ApplicationUserId)
            .NotEmpty()
            .WithMessage("Application user ID is required");
    }
}
