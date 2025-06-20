using FluentValidation;

namespace DietManagementSystem.Application.Features.Clients.DeleteClient;

public class DeleteClientCommandValidator : AbstractValidator<DeleteClientCommand>
{
    public DeleteClientCommandValidator()
    {
        RuleFor(query => query.Id)
            .NotEmpty()
            .WithMessage("Client ID must not be empty.")
            .Must(id => id != Guid.Empty)
            .WithMessage("Client ID must be a valid GUID.");
    }
}
