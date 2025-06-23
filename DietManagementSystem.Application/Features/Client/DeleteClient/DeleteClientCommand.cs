using MediatR;

namespace DietManagementSystem.Application.Features.Client.DeleteClient;

public class DeleteClientCommand : IRequest<Result<DeleteClientCommandResponse>>
{
    public Guid ClientId { get; set; }
}
