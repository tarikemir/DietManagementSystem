using MediatR;

namespace DietManagementSystem.Application.Features.Dietitian.DeleteDietitian;

public class DeleteDietitianCommand : IRequest<Result<DeleteDietitianCommandResponse>>
{
    public Guid Id { get; set; }
}
