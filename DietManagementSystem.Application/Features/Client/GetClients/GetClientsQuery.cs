using MediatR;

namespace DietManagementSystem.Application.Features.Client.GetClients;

public class GetClientsQuery : IRequest<Result<List<GetClientsQueryResponse>>>
{
}
