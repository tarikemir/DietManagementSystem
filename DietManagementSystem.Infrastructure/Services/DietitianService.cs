using DietManagementSystem.Application.Common;
using DietManagementSystem.Application.Features.Dietitian.CreateDietitian;
using DietManagementSystem.Application.Features.Dietitian.GetDietitianById;
using DietManagementSystem.Application.Features.Dietitian.GetDietitians;
using DietManagementSystem.Application.Features.Dietitian.UpdateDietitian;
using DietManagementSystem.Application.Services;
using DietManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DietManagementSystem.Infrastructure.Services;

public class DietitianService : IDietitianService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoggingService _loggingService;

    public DietitianService(IUnitOfWork unitOfWork, ILoggingService loggingService)
    {
        _unitOfWork = unitOfWork;
        _loggingService = loggingService;
    }

    public async Task<Result<CreateDietitianCommandResponse>> CreateDietitianAsync(CreateDietitianCommand command)
    {
        try
        {
            var dietitian = new Dietitian
            {
                FirstName = command.FirstName,
                LastName = command.LastName,
            };

            await _unitOfWork.Dietitians.AddAsync(dietitian);
            await _unitOfWork.SaveChangesAsync();

            return Result<CreateDietitianCommandResponse>.Success(new CreateDietitianCommandResponse
            {
                Id = dietitian.Id,
                FirstName = dietitian.FirstName,
                LastName = dietitian.LastName,
                ApplicationUserId = dietitian.ApplicationUserId
            });
        }
        catch (Exception ex)
        {
            _loggingService.LogError(ex, "Error creating dietitian");
            return Result<CreateDietitianCommandResponse>.Failure($"Failed to create dietitian: {ex.Message}");
        }
    }

    public async Task<Result<UpdateDietitianCommandResponse>> UpdateDietitianAsync(UpdateDietitianCommand command)
    {
        try
        {
            var dietitian = await _unitOfWork.Dietitians.FirstOrDefaultAsync(d => d.Id == command.Id);
            if (dietitian == null)
            {
                _loggingService.LogWarning("Dietitian not found with ID: {DietitianId}", command.Id);
                return Result<UpdateDietitianCommandResponse>.Failure("Dietitian not found");
            }

            dietitian.FirstName = command.FirstName;
            dietitian.LastName = command.LastName;
            dietitian.ApplicationUserId = command.ApplicationUserId;

            await _unitOfWork.SaveChangesAsync();

            return Result<UpdateDietitianCommandResponse>.Success(new UpdateDietitianCommandResponse
            {
                Id = dietitian.Id,
                FirstName = dietitian.FirstName,
                LastName = dietitian.LastName,
                ApplicationUserId = dietitian.ApplicationUserId
            });
        }
        catch (Exception ex)
        {
            _loggingService.LogError(ex, "Error updating dietitian with ID: {DietitianId}", command.Id);
            return Result<UpdateDietitianCommandResponse>.Failure($"Failed to update dietitian: {ex.Message}");
        }
    }

    public async Task<Result> DeleteDietitianAsync(Guid id)
    {
        try
        {
            var dietitian = await _unitOfWork.Dietitians.FirstOrDefaultAsync(d => d.Id == id);
            if (dietitian == null)
            {
                _loggingService.LogWarning("Dietitian not found for deletion with ID: {DietitianId}", id);
                return Result.Failure("Dietitian not found");
            }

            await _unitOfWork.Dietitians.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }
        catch (Exception ex)
        {
            _loggingService.LogError(ex, "Error deleting dietitian with ID: {DietitianId}", id);
            return Result.Failure($"Failed to delete dietitian: {ex.Message}");
        }
    }

    public async Task<Result<GetDietitianByIdQueryResponse>> GetDietitianAsync(Guid id)
    {
        try
        {
            var dietitian = await _unitOfWork.Dietitians
                .Query().Include(d => d.Clients)
                .Include(d => d.ApplicationUser)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (dietitian == null)
            {
                _loggingService.LogWarning("Dietitian not found with ID: {DietitianId}", id);
                return Result<GetDietitianByIdQueryResponse>.Failure("Dietitian not found");
            }

            _loggingService.LogDebug("Successfully retrieved dietitian with ID: {DietitianId}, ClientCount: {ClientCount}", 
                dietitian.Id, dietitian.Clients.Count);

            return Result<GetDietitianByIdQueryResponse>.Success(new GetDietitianByIdQueryResponse
            {
                Id = dietitian.Id,
                FirstName = dietitian.FirstName,
                LastName = dietitian.LastName,
                Email = dietitian.ApplicationUser.Email,
                ApplicationUserId = dietitian.ApplicationUserId,
                ClientCount = dietitian.Clients.Count
            });
        }
        catch (Exception ex)
        {
            _loggingService.LogError(ex, "Error retrieving dietitian with ID: {DietitianId}", id);
            return Result<GetDietitianByIdQueryResponse>.Failure($"Failed to retrieve dietitian: {ex.Message}");
        }
    }

    public async Task<Result<List<GetDietitiansQueryResponse>>> GetAllDietitiansAsync()
    {
        try
        {

            var dietitians = await _unitOfWork.Dietitians
                .Query()
                .Include(d => d.Clients)
                .Include(d => d.ApplicationUser)
                .Select(d => new GetDietitiansQueryResponse
                {
                    Id = d.Id,
                    FirstName = d.FirstName,
                    LastName = d.LastName,
                    Email = d.ApplicationUser.Email,
                    ApplicationUserId = d.ApplicationUserId,
                    ClientCount = d.Clients.Count
                })
                .ToListAsync();

            return Result<List<GetDietitiansQueryResponse>>.Success(dietitians);
        }
        catch (Exception ex)
        {
            _loggingService.LogError(ex, "Error retrieving all dietitians");
            return Result<List<GetDietitiansQueryResponse>>.Failure($"Failed to retrieve dietitians: {ex.Message}");
        }
    }
}