using MediatR;
using DietManagementSystem.Application.Common;
using DietManagementSystem.Application.Dtos;
using DietManagementSystem.Domain.Entities;

namespace DietManagementSystem.Application.Features.Clients.Create;

public class CreateClientCommandHandler : IRequestHandler<CreateClientCommand, Result<ClientDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateClientCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ClientDto>> Handle(CreateClientCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var existingClients = _unitOfWork.Clients.FindAsync(c => c.Email == request.Email);
            if (existingClients.Any())
            {
                return Result<ClientDto>.Failure("A client with this email already exists.");
            }

            var client = new Client
            {
                Id = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                InitialWeight = request.InitialWeight,
                DietitianId = request.DietitianId,
                ApplicationUserId = request.ApplicationUserId
            };

            await _unitOfWork.Clients.AddAsync(client);
            await _unitOfWork.SaveChangesAsync();

            var clientDto = new ClientDto
            {
                Id = client.Id,
                FirstName = client.FirstName,
                LastName = client.LastName,
                Email = client.Email,
                PhoneNumber = client.PhoneNumber,
                InitialWeight = client.InitialWeight,
                DietitianId = client.DietitianId,
                ApplicationUserId = client.ApplicationUserId,
                CreatedAt = client.CreatedAt,
                UpdatedAt = client.UpdatedAt
            };

            return Result<ClientDto>.Success(clientDto);
        }
        catch (Exception ex)
        {
            return Result<ClientDto>.Failure($"Failed to create client: {ex.Message}");
        }
    }
}
