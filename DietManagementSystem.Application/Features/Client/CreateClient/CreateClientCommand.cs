using MediatR;

namespace DietManagementSystem.Application.Features.Client.CreateClient;

public class CreateClientCommand : IRequest<Result<CreateClientCommandResponse>>
{
    public string Email { get; init; } = null!;
    public string Password { get; init; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public double InitialWeight { get; set; }
    public Guid DietitianId { get; set; }
}
