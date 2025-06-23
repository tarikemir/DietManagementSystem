using MediatR;

namespace DietManagementSystem.Application.Features.Client.GetClientsByDietitian;

public class GetClientsByDietitianQuery : IRequest<Result<List<GetClientsByDietitianQueryResponse>>>
{
    public Guid DietitianId { get; set; }
}