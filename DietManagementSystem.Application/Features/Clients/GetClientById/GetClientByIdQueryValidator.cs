using FluentValidation;

namespace DietManagementSystem.Application.Features.Clients.GetClientById;

public class GetClientByIdQueryValidator : AbstractValidator<GetClientByIdQuery>
{
    public GetClientByIdQueryValidator()
    {
        RuleFor(query => query.Id)
            .NotEmpty()
            .WithMessage("Client ID must not be empty.")
            .Must(id => id != Guid.Empty)
            .WithMessage("Client ID must be a valid GUID.");
    }
}
