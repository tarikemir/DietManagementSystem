using DietManagementSystem.Application.Common;
using DietManagementSystem.Application.Features.Client.GetClientsByDietitian;
using DietManagementSystem.Application.Features.DietPlan.GetDietPlan;
using DietManagementSystem.Application.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DietManagementSystem.Application.Features.Client.GetClients;

public class GetClientsQueryHandler : IRequestHandler<GetClientsQuery, Result<List<GetClientsQueryResponse>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetClientsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<Result<List<GetClientsQueryResponse>>> Handle(GetClientsQuery request, CancellationToken cancellationToken)
    {
        var clients = await _unitOfWork.Clients.Query()
            .Include(client => client.Dietitian)
            .Include(client => client.ApplicationUser)
            .Select(client => new GetClientsQueryResponse
            {
                Id = client.Id,
                FirstName = client.FirstName,
                LastName = client.LastName,
                Email = client.ApplicationUser.Email,
                InitialWeight = client.InitialWeight,
                DietitianName = client.Dietitian.FirstName + " " + client.Dietitian.LastName,
                DietitianId = client.DietitianId,
                ApplicationUserId = client.ApplicationUserId,
            }).ToListAsync(cancellationToken);

        if (clients is null)
        {
            return Result<List<GetClientsQueryResponse>>.Failure("Clients not found.");
        }
        return Result<List<GetClientsQueryResponse>>.Success(clients);
    }
}
