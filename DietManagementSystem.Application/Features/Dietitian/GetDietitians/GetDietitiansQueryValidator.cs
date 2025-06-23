using FluentValidation;

namespace DietManagementSystem.Application.Features.Dietitian.GetDietitians;

public class GetDietitiansQueryValidator : AbstractValidator<GetDietitiansQuery>
{
    public GetDietitiansQueryValidator()
    {
        // No validation rules needed for getting all dietitians
    }
}
