using DietManagementSystem.Application.Features.Dietitian.CreateDietitian;
using DietManagementSystem.Application.Features.Dietitian.GetDietitianById;
using DietManagementSystem.Application.Features.Dietitian.GetDietitians;
using DietManagementSystem.Application.Features.Dietitian.UpdateDietitian;

namespace DietManagementSystem.Application.Services;

public interface IDietitianService
{
    Task<Result<CreateDietitianCommandResponse>> CreateDietitianAsync(CreateDietitianCommand command);
    Task<Result<UpdateDietitianCommandResponse>> UpdateDietitianAsync(UpdateDietitianCommand command);
    Task<Result> DeleteDietitianAsync(Guid id);
    Task<Result<GetDietitianByIdQueryResponse>> GetDietitianAsync(Guid id);
    Task<Result<List<GetDietitiansQueryResponse>>> GetAllDietitiansAsync();
}
