using DietManagementSystem.Application.Dtos;
using MediatR;

namespace DietManagementSystem.Application.Features.Clients.UpdateClient;

public class UpdateClientCommand : IRequest<Result<ClientDto>>
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public double InitialWeight { get; set; }
}
