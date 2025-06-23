using FluentValidation;

namespace DietManagementSystem.Application.Features.Client.GetClientsByDietitian;

public class GetClientsByDietitianQueryValidator : AbstractValidator<GetClientsByDietitianQuery>
{
    public GetClientsByDietitianQueryValidator()
    {
        RuleFor(c => c.DietitianId)
            .NotEmpty()
            .WithMessage("Client ID cannot be empty.")
            .NotNull()
            .WithMessage("Client ID cannot be null.")
            .Must(id => id != Guid.Empty)
            .WithMessage("Client ID must be a valid GUID.");
    }
}
