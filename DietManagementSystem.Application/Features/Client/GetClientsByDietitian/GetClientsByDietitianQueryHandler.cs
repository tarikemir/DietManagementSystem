using DietManagementSystem.Application.Features.Client.GetClient;
using DietManagementSystem.Application.Services;
using MediatR;

namespace DietManagementSystem.Application.Features.Client.GetClientsByDietitian;

public class GetClientsByDietitianQueryHandler : IRequestHandler<GetClientsByDietitianQuery, Result<List<GetClientsByDietitianQueryResponse>>>
{
    private readonly IClientService _clientService;

    public GetClientsByDietitianQueryHandler(IClientService clientService)
    {
        _clientService = clientService;
    }

    public async Task<Result<List<GetClientsByDietitianQueryResponse>>> Handle(GetClientsByDietitianQuery request, CancellationToken cancellationToken)
    {
        var client = await _clientService.GetClientsByDietitianAsync(request.DietitianId);
        if (client is null)
        {
            return Result<List<GetClientsByDietitianQueryResponse>>.Failure("Clients not found.");
        }
        return Result<List<GetClientsByDietitianQueryResponse>>.Success(client.Value);
    }
}
