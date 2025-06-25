using DietManagementSystem.Application.Common;
using DietManagementSystem.Domain.Enums;

namespace DietManagementSystem.Application.Features.Auth.RegisterDietitian;

public class RegisterDietitianCommandResponse : IAuthResponse
{
    public string Token { get; set; } = null!;
    public DateTime Expiration { get; set; }
}
