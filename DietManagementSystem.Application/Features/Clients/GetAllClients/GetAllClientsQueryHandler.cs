using DietManagementSystem.Application.Common;
using DietManagementSystem.Application.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DietManagementSystem.Application.Features.Clients.GetAllClients;

public class GetAllClientsQueryHandler : IRequestHandler<GetAllClientsQuery, Result<List<ClientDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllClientsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<ClientDto>>> Handle(GetAllClientsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _unitOfWork.Clients
                        .GetAllAsync()
                        .Skip((request.Page - 1) * request.Size)
                        .Take(request.Size)
                        .Select(c => new ClientDto
                        {
                            Id = c.Id,
                            FirstName = c.FirstName,
                            LastName = c.LastName,
                            Email = c.Email,
                            PhoneNumber = c.PhoneNumber,
                            InitialWeight = c.InitialWeight
                        })
                        .ToListAsync(cancellationToken);

            if (result is null)
                return Result<List<ClientDto>>.Failure($"Failed to fetch clients");

            return Result<List<ClientDto>>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<List<ClientDto>>.Failure($"Failed to fetch clients: {ex.Message}");
        }
    }
}
