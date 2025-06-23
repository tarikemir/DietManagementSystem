using MediatR;

namespace DietManagementSystem.Application.Features.Dietitian.GetDietitians;

public class GetDietitiansQuery : IRequest<Result<List<GetDietitiansQueryResponse>>>
{
}
