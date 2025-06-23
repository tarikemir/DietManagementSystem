using MediatR;

namespace DietManagementSystem.Application.Features.Dietitian.GetDietitianById;

public class GetDietitianByIdQuery : IRequest<Result<GetDietitianByIdQueryResponse>>
{
    public Guid Id { get; set; }
}
