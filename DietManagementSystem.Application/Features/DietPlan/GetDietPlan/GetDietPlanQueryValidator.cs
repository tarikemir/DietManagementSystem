using FluentValidation;

namespace DietManagementSystem.Application.Features.DietPlan.GetDietPlan;

public class GetDietPlanQueryValidator : AbstractValidator<GetDietPlanQuery>
{
    public GetDietPlanQueryValidator()
    {
        RuleFor(dp => dp.Id)
            .NotEmpty().WithMessage("Id is required.");
    }
}
