using FluentValidation;

namespace DietManagementSystem.Application.Features.DietPlan.GetDietPlansByClient;

public class GetDietPlansByClientQueryValidator : AbstractValidator<GetDietPlansByClientQuery>
{
    public GetDietPlansByClientQueryValidator()
    {
        RuleFor(x => x.ClientId)
            .NotEmpty().WithMessage("Client ID is required.")
            .NotNull().WithMessage("Client ID cannot be null.");
    }
}
