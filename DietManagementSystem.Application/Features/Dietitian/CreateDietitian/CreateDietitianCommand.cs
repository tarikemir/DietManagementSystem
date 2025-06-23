using MediatR;

namespace DietManagementSystem.Application.Features.Dietitian.CreateDietitian;

public class CreateDietitianCommand : IRequest<Result<CreateDietitianCommandResponse>>
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
}
