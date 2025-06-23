using DietManagementSystem.Application.Services;
using MediatR;

namespace DietManagementSystem.Application.Features.Client.GetClient;

public class GetClientByIdQueryHandler : IRequestHandler<GetClientByIdQuery, Result<GetClientByIdQueryResponse>>
{
    private readonly IClientService _clientService;
    public GetClientByIdQueryHandler(IClientService clientService)
    {
        _clientService = clientService;
    }
    public async Task<Result<GetClientByIdQueryResponse>> Handle(GetClientByIdQuery request, CancellationToken cancellationToken)
    {
        var client = await _clientService.GetClientAsync(request.ClientId);
        if (client is null)
        {
            return Result<GetClientByIdQueryResponse>.Failure("Client not found.");
        }
        return Result<GetClientByIdQueryResponse>.Success(client.Value);
    }
}
