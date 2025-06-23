using DietManagementSystem.Application.Features.Auth.Login;
using DietManagementSystem.Application.Features.Auth.RegisterClient;
using DietManagementSystem.Application.Features.Auth.RegisterDietitian;

namespace DietManagementSystem.Application.Services;

public interface IAuthService
{
    Task<Result<LoginCommandResponse>> LoginAsync(LoginCommand request);
    Task<Result<RegisterClientCommandResponse>> RegisterClientAsync(RegisterClientCommand request);
    Task<Result<RegisterDietitianCommandResponse>> RegisterDietitianAsync(RegisterDietitianCommand request);
}
