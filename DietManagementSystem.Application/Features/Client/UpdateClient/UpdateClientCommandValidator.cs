using FluentValidation;

namespace DietManagementSystem.Application.Features.Client.UpdateClient;

public class UpdateClientCommandValidator : AbstractValidator<UpdateClientCommand>
{
    public UpdateClientCommandValidator()
    {
        RuleFor(c => c.FirstName)
            .NotEmpty().WithMessage("Client's First name cannot be empty.")
            .MaximumLength(50).WithMessage("Client's First name cannot exceed 50 characters.");
        RuleFor(c => c.LastName)
            .NotEmpty().WithMessage("Client's Last name cannot be empty.")
            .MaximumLength(50).WithMessage("Client's Last name cannot exceed 50 characters.");
        RuleFor(c => c.InitialWeight)
            .GreaterThan(0).WithMessage("Client's Initial weight must be greater than 0.");
    }
}