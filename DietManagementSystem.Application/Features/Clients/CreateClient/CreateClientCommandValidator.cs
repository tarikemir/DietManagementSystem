using FluentValidation;

namespace DietManagementSystem.Application.Features.Clients.Create;

public class CreateClientCommandValidator : AbstractValidator<CreateClientCommand>
{
    public CreateClientCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.")
            .Matches(@"^[a-zA-Z\s]+$").WithMessage("First name can only contain letters and spaces.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.")
            .Matches(@"^[a-zA-Z\s]+$").WithMessage("Last name can only contain letters and spaces.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Please provide a valid email address.")
            .MaximumLength(100).WithMessage("Email cannot exceed 100 characters.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^[\+]?[1-9][\d]{0,15}$").WithMessage("Please provide a valid phone number.")
            .MaximumLength(20).WithMessage("Phone number cannot exceed 20 characters.");

        RuleFor(x => x.InitialWeight)
            .GreaterThan(0).WithMessage("Initial weight must be greater than 0.")
            .LessThanOrEqualTo(1000).WithMessage("Initial weight cannot exceed 1000 kg.");

        RuleFor(x => x.DietitianId)
            .NotEmpty().WithMessage("Dietitian ID is required.");

        RuleFor(x => x.ApplicationUserId)
            .NotEmpty().WithMessage("Application user ID is required.");
    }
}
