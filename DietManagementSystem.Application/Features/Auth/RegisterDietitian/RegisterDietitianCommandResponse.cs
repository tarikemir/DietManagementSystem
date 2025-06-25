using DietManagementSystem.Application.Common;
using DietManagementSystem.Domain.Enums;

namespace DietManagementSystem.Application.Features.Auth.RegisterDietitian;

public class RegisterDietitianCommandResponse : IAuthResponse
{
    public string Token { get; set; } = null!;
    public DateTime Expiration { get; set; }
    public Guid UserId { get; set; }
    public string Email { get; set; } = null!;
    public string UserType { get; set; }
}
