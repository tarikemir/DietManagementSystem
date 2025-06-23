using MediatR;

namespace DietManagementSystem.Application.Features.Client.UpdateClient;

public class UpdateClientCommand : IRequest<Result<UpdateClientCommandResponse>>
{
    public Guid ClientId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public double InitialWeight { get; set; }
}
