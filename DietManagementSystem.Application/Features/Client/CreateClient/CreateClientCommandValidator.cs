using FluentValidation;

namespace DietManagementSystem.Application.Features.Client.CreateClient;

public class CreateClientCommandValidator : AbstractValidator<CreateClientCommand>
{
    public CreateClientCommandValidator()
    {
        //write validations
        RuleFor(c => c.FirstName)
            .NotEmpty().WithMessage("Client's First name cannot be empty.")
            .MaximumLength(50).WithMessage("Client's First name cannot exceed 50 characters.");
        RuleFor(c => c.LastName)
            .NotEmpty().WithMessage("Client's Last name cannot be empty.")
            .MaximumLength(50).WithMessage("Client's Last name cannot exceed 50 characters.");
        RuleFor(c => c.InitialWeight)
            .GreaterThan(0).WithMessage("Client's Initial weight must be greater than 0.");
        RuleFor(c => c.DietitianId)
            .NotEmpty().WithMessage("Dietitian ID cannot be empty.")
            .NotEqual(Guid.Empty).WithMessage("Dietitian ID cannot be an empty GUID.");
        RuleFor(c => c.ApplicationUserId)
            .NotEmpty().WithMessage("Application User ID cannot be empty.")
            .NotEqual(Guid.Empty).WithMessage("Application User ID cannot be an empty GUID.");
    }
}