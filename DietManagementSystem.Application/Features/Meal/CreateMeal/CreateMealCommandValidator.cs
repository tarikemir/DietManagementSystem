using FluentValidation;

namespace DietManagementSystem.Application.Features.Meal.CreateMeal;

public class CreateMealCommandValidator : AbstractValidator<CreateMealCommand>
{
    public CreateMealCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");
        RuleFor(x => x.ScheduledDate)
            .NotNull().WithMessage("Scheduled date is required.");
        RuleFor(x => x.StartTime)
            .NotEmpty().WithMessage("Start time is required.")
            .LessThan(x => x.EndTime).WithMessage("Start time must be before end time.");
        RuleFor(x => x.EndTime)
            .NotEmpty().WithMessage("End time is required.")
            .GreaterThan(x => x.StartTime).WithMessage("End time must be after start time.");
        RuleFor(x => x.Contents)
            .NotEmpty().WithMessage("Contents are required.")
            .MaximumLength(500).WithMessage("Contents must not exceed 500 characters.");
        RuleFor(x => x.DietPlanId)
            .NotEmpty().WithMessage("Diet plan ID is required.")
            .Must(id => id != Guid.Empty).WithMessage("Diet plan ID must be a valid GUID.");
        RuleFor(x => x)
            .Must(x => x.StartTime < x.EndTime)
            .WithMessage("Start time must be before end time.");
    }
}
