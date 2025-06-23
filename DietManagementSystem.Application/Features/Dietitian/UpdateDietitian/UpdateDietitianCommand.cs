using MediatR;

namespace DietManagementSystem.Application.Features.Dietitian.UpdateDietitian;

public class UpdateDietitianCommand : IRequest<Result<UpdateDietitianCommandResponse>>
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public Guid ApplicationUserId { get; set; }
}
