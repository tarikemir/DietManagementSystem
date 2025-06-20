using MediatR;

namespace DietManagementSystem.Application.Features.Clients.DeleteClient;

public class DeleteClientCommand : IRequest<Result>
{
    public Guid Id { get; set; }
}
