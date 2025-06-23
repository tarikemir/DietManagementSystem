using DietManagementSystem.Application.Features.Client.CreateClient;
using DietManagementSystem.Application.Features.Client.GetClient;
using DietManagementSystem.Application.Features.Client.GetClientsByDietitian;
using DietManagementSystem.Application.Features.Client.UpdateClient;

namespace DietManagementSystem.Application.Services;

public interface IClientService
{
    Task<Result<CreateClientCommandResponse>> CreateClientAsync(CreateClientCommand request);
    Task<Result<UpdateClientCommandResponse>> UpdateClientAsync(UpdateClientCommand request);
    Task<Result> DeleteClientAsync(Guid id);
    Task<Result<GetClientByIdQueryResponse>> GetClientAsync(Guid id);
    Task<Result<List<GetClientsByDietitianQueryResponse>>> GetClientsByDietitianAsync(Guid dietitianId);
}