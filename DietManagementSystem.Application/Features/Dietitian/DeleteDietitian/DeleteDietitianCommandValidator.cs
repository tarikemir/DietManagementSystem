using FluentValidation;

namespace DietManagementSystem.Application.Features.Dietitian.DeleteDietitian;

public class DeleteDietitianCommandValidator : AbstractValidator<DeleteDietitianCommand>
{
    public DeleteDietitianCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Dietitian ID is required");
    }
}
