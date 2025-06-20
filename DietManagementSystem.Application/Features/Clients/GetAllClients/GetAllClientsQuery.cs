using DietManagementSystem.Application.Dtos;
using MediatR;

namespace DietManagementSystem.Application.Features.Clients.GetAllClients;

public class GetAllClientsQuery : IRequest<Result<List<ClientDto>>>
{
    public int Page { get; set; } = 1;
    public int Size { get; set; } = 10;
}
