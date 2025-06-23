using FluentValidation;

namespace DietManagementSystem.Application.Features.Dietitian.GetDietitianById;

public class GetDietitianByIdQueryValidator : AbstractValidator<GetDietitianByIdQuery>
{
    public GetDietitianByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Dietitian ID is required");
    }
}
