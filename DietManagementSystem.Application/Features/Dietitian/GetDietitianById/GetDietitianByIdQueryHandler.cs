using DietManagementSystem.Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DietManagementSystem.Application.Features.Dietitian.GetDietitianById;

public class GetDietitianByIdQueryHandler
    : IRequestHandler<GetDietitianByIdQuery, Result<GetDietitianByIdQueryResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetDietitianByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<GetDietitianByIdQueryResponse>> Handle(
        GetDietitianByIdQuery request,
        CancellationToken cancellationToken)
    {
        var dietitian = await _unitOfWork.Dietitians
            .Query()
            .Include(d => d.Clients)
            .Include(d => d.DietPlans)
            .FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

        if (dietitian == null)
            return Result<GetDietitianByIdQueryResponse>.Failure("Dietitian not found");

        return Result<GetDietitianByIdQueryResponse>.Success(new GetDietitianByIdQueryResponse
        {
            Id = dietitian.Id,
            FirstName = dietitian.FirstName,
            LastName = dietitian.LastName,
            ApplicationUserId = dietitian.ApplicationUserId,
            ClientCount = dietitian.Clients.Count,
            DietPlanCount = dietitian.DietPlans.Count,
            CreatedAt = dietitian.CreatedAt,
            UpdatedAt = dietitian.UpdatedAt
        });
    }
}
