using MediatR;
using DietManagementSystem.Application.Dtos;

namespace DietManagementSystem.Application.Features.Clients.Create;
public class CreateClientCommand : IRequest<Result<ClientDto>>
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public double InitialWeight { get; set; }
    public Guid DietitianId { get; set; }
    public Guid ApplicationUserId { get; set; }
}
