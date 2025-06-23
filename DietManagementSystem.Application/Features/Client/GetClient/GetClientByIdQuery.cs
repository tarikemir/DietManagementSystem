using MediatR;

namespace DietManagementSystem.Application.Features.Client.GetClient;

public class GetClientByIdQuery : IRequest<Result<GetClientByIdQueryResponse>>
{
    public Guid ClientId { get; set; }
}
