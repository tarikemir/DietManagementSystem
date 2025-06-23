using MediatR;

namespace DietManagementSystem.Application.Features.Client.CreateClient;

public class CreateClientCommand : IRequest<Result<CreateClientCommandResponse>>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public double InitialWeight { get; set; }
    public Guid DietitianId { get; set; }
    public Guid ApplicationUserId { get; set; }
}
