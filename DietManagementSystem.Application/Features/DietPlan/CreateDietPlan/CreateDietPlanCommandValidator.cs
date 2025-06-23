using FluentValidation;

namespace DietManagementSystem.Application.Features.DietPlan.CreateDietPlan;

public class CreateDietPlanCommandValidator : AbstractValidator<CreateDietPlanCommand>
{
    public CreateDietPlanCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(100).WithMessage("Title cannot exceed 100 characters");
        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start date is required")
            .LessThanOrEqualTo(x => x.EndDate).WithMessage("Start date must be before or equal to end date");
        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("End date is required")
            .GreaterThanOrEqualTo(x => x.StartDate).WithMessage("End date must be after or equal to start date");
        RuleFor(x => x.InitialWeight)
            .GreaterThan(0).WithMessage("Initial weight must be greater than zero");
        RuleFor(x => x.TargetWeight)
            .GreaterThan(0).WithMessage("Target weight must be greater than zero");
        RuleFor(x => x.ClientId)
            .NotEmpty().WithMessage("Client ID is required");
        RuleFor(x => x.DietitianId)
            .NotEmpty().WithMessage("Dietitian ID is required");
    }
}
