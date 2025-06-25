using DietManagementSystem.Application.Common;
using DietManagementSystem.Domain.Enums;

namespace DietManagementSystem.Application.Features.Auth.Login;

public class LoginCommandResponse: IAuthResponse
{
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
}
