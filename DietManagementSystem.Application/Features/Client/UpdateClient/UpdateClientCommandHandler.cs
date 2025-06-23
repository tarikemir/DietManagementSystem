using DietManagementSystem.Application.Features.DietPlan.CreateDietPlan;
using DietManagementSystem.Application.Services;
using MediatR;

namespace DietManagementSystem.Application.Features.Client.UpdateClient;

public class UpdateClientCommandHandler : IRequestHandler<UpdateClientCommand, Result<UpdateClientCommandResponse>>
{
    private readonly IClientService _clientService;

    public UpdateClientCommandHandler(IClientService clientService)
    {
        _clientService = clientService;
    }

    public async Task<Result<UpdateClientCommandResponse>> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _clientService.UpdateClientAsync(request);
            if (result.IsFailure)
            {
                return Result<UpdateClientCommandResponse>.Failure(result.Error);
            }
            return Result<UpdateClientCommandResponse>.Success(result.Value);
        }
        catch (Exception e)
        {
            return Result<UpdateClientCommandResponse>.Failure($"An error occurred while processing the request: {e.Message}");
        }
    }
}