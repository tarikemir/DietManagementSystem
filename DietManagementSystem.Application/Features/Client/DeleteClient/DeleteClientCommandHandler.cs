using DietManagementSystem.Application.Features.DietPlan.CreateDietPlan;
using DietManagementSystem.Application.Services;
using MediatR;

namespace DietManagementSystem.Application.Features.Client.DeleteClient;

public class DeleteClientCommandHandler : IRequestHandler<DeleteClientCommand, Result<DeleteClientCommandResponse>>
{
    public readonly IClientService _clientService;

    public DeleteClientCommandHandler(IClientService clientService)
    {
        _clientService = clientService ?? throw new ArgumentNullException("Client server cannot be null!");
    }

    public async Task<Result<DeleteClientCommandResponse>> Handle(DeleteClientCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _clientService.DeleteClientAsync(request.ClientId);
            if (result.IsFailure)
            {
                return Result<DeleteClientCommandResponse>.Failure(result.Error);
            }
            return Result<DeleteClientCommandResponse>.Success(null);
        }
        catch (Exception e)
        {
            return Result<DeleteClientCommandResponse>.Failure($"An error occurred while processing the request: {e.Message}");
        }
    }
}
