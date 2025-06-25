using FluentValidation;
using FluentValidation.Validators;

namespace DietManagementSystem.Application.Features.DietPlan.UpdateDietPlan;

public class UpdateDietPlanCommandValidator : AbstractValidator<UpdateDietPlanCommand>
{
    public UpdateDietPlanCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Diet plan ID is required.")
            .NotEqual(Guid.Empty).WithMessage("Diet plan ID cannot be empty.");
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");
        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start date is required.")
            .GreaterThan(DateTime.MinValue).WithMessage("Start date must be a valid date.");
        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("End date is required.")
            .GreaterThan(x => x.StartDate).WithMessage("End date must be after the start date.");
        RuleFor(x => x.InitialWeight)
            .GreaterThan(0).WithMessage("Initial weight must be greater than zero.");
        RuleFor(x => x.TargetWeight)
            .GreaterThan(0).WithMessage("Target weight must be greater than zero.")
            .GreaterThan(x => x.InitialWeight).WithMessage("Target weight must be greater than initial weight.");
    }
}