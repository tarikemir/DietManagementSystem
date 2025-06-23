using DietManagementSystem.Application.Features.DietPlan.CreateDietPlan;
using DietManagementSystem.Application.Services;
using MediatR;

namespace DietManagementSystem.Application.Features.Client.CreateClient;

public class CreateClientCommandHandler : IRequestHandler<CreateClientCommand, Result<CreateClientCommandResponse>>
{
    private readonly IClientService _clientService;

    public CreateClientCommandHandler(IClientService clientService)
    {
        _clientService = clientService ?? throw new ArgumentNullException("Client service Cannot be null");
    }

    public async Task<Result<CreateClientCommandResponse>> Handle(CreateClientCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _clientService.CreateClientAsync(request);
            if (result.IsFailure)
            {
                return Result<CreateClientCommandResponse>.Failure(result.Error);
            }
            return Result<CreateClientCommandResponse>.Success(result.Value);
        }
        catch (Exception e)
        {
            return Result<CreateClientCommandResponse>.Failure($"An error occurred while processing the request: {e.Message}");
        }
    }
}
