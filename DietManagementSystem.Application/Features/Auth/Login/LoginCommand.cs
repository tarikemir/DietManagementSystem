using MediatR;

namespace DietManagementSystem.Application.Features.Auth.Login;

public class LoginCommand : IRequest<Result<LoginCommandResponse>>
{
    public string Email { get; set; }
    public string Password { get; set; }
}