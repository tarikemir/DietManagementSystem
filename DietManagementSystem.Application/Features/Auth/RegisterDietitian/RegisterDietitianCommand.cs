using MediatR;

namespace DietManagementSystem.Application.Features.Auth.RegisterDietitian;

public class RegisterDietitianCommand : IRequest<Result<RegisterDietitianCommandResponse>>
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
}
