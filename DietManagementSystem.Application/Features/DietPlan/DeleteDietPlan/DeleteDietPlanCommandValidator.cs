using FluentValidation;

namespace DietManagementSystem.Application.Features.DietPlan.DeleteDietPlan;

public class DeleteDietPlanCommandValidator : AbstractValidator<DeleteDietPlanCommand>
{
    public DeleteDietPlanCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Diet plan ID is required.");
    }
}
