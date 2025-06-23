using DietManagementSystem.Application.Features.DietPlan.CreateDietPlan;
using DietManagementSystem.Application.Services;
using MediatR;

namespace DietManagementSystem.Application.Features.Client.CreateClient;

public class CreateClientCommandHandler : IRequestHandler<CreateClientCommand, Result<CreateClientCommandResponse>>
{
    private readonly IClientService _clientService;
    private readonly IAuthService _authService;

    public CreateClientCommandHandler(IClientService clientService, IAuthService authService)
    {
        _clientService = clientService ?? throw new ArgumentNullException("Client service Cannot be null");
        _authService = authService ?? throw new ArgumentNullException("Auth service Cannot be null");
    }

    public async Task<Result<CreateClientCommandResponse>> Handle(CreateClientCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var createdUser = await _authService.RegisterClientAsync(new Auth.RegisterClient.RegisterClientCommand
            {
                Email = request.Email,
                Password = request.Password,
                FirstName = request.FirstName,
                LastName = request.LastName,
                DietitianId = request.DietitianId
            });

            var result = await _clientService.GetClientAsync(createdUser.Value.UserId);

            if (result.IsFailure || result is null)
            {
                return Result<CreateClientCommandResponse>.Failure(result.Error);
            }
            return Result<CreateClientCommandResponse>.Success(new CreateClientCommandResponse
            {
                Id = result.Value.Id,
                FirstName = result.Value.FirstName,
                LastName = result.Value.LastName,
                InitialWeight = result.Value.InitialWeight,
                DietitianId = result.Value.DietitianId
            });
        }
        catch (Exception e)
        {
            return Result<CreateClientCommandResponse>.Failure($"An error occurred while processing the request: {e.Message}");
        }
    }
}
