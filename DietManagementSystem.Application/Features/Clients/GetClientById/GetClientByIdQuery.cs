using DietManagementSystem.Application.Dtos;
using MediatR;

namespace DietManagementSystem.Application.Features.Clients.GetClientById;

public class GetClientByIdQuery : IRequest<Result<ClientDto>>
{
    public Guid Id { get; set; }
}
