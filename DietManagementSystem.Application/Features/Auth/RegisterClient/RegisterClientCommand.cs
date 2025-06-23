using MediatR;

namespace DietManagementSystem.Application.Features.Auth.RegisterClient;

public class RegisterClientCommand : IRequest<Result<RegisterClientCommandResponse>>
{
    public string Email { get; init; } = null!;
    public string Password { get; init; } = null!;
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
    public double InitialWeight { get; init; }
    public Guid DietitianId { get; init; }
}
