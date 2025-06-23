using FluentValidation;

namespace DietManagementSystem.Application.Features.Client.DeleteClient;

public class DeleteClientCommandValidator : AbstractValidator<DeleteClientCommand>
{
    public DeleteClientCommandValidator()
    {
        RuleFor(c => c.ClientId)
            .NotEmpty()
            .WithMessage("Client ID cannot be empty.")
            .NotNull()
            .WithMessage("Client ID cannot be null.")
            .Must(id => id != Guid.Empty)
            .WithMessage("Client ID must be a valid GUID.");
    }
}
