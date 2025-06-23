using DietManagementSystem.Application.Common;
using DietManagementSystem.Domain.Enums;

namespace DietManagementSystem.Application.Features.Auth.Login;

public class LoginCommandResponse: IAuthResponse
{
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
    public Guid UserId { get; set; }
    public string Email { get; set; }
    public UserType UserType { get; set; }
}
