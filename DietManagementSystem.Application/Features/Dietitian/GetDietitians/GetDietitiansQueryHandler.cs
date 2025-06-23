using DietManagementSystem.Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DietManagementSystem.Application.Features.Dietitian.GetDietitians;

public class GetDietitiansQueryHandler
    : IRequestHandler<GetDietitiansQuery, Result<List<GetDietitiansQueryResponse>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetDietitiansQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<GetDietitiansQueryResponse>>> Handle(
        GetDietitiansQuery request,
        CancellationToken cancellationToken)
    {
        var dietitians = await _unitOfWork.Dietitians
            .Query()
            .Include(d => d.Clients)
            .Include(d => d.DietPlans)
            .Select(d => new GetDietitiansQueryResponse
            {
                Id = d.Id,
                FirstName = d.FirstName,
                LastName = d.LastName,
                ApplicationUserId = d.ApplicationUserId,
                ClientCount = d.Clients.Count,
                DietPlanCount = d.DietPlans.Count,
                CreatedAt = d.CreatedAt,
                UpdatedAt = d.UpdatedAt
            })
            .ToListAsync(cancellationToken);

        return Result<List<GetDietitiansQueryResponse>>.Success(dietitians);
    }
}
