using DietManagementSystem.Application.Common;
using DietManagementSystem.Application.Services;
using DietManagementSystem.Domain.Enums;
using DietManagementSystem.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace DietManagementSystem.Application.Features.Dietitian.CreateDietitian;

public class CreateDietitianCommandHandler
    : IRequestHandler<CreateDietitianCommand, Result<CreateDietitianCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthService _authService;

    public CreateDietitianCommandHandler(IUnitOfWork unitOfWork, IAuthService authService)
    {
        _unitOfWork = unitOfWork;
        _authService = authService;
    }

    public async Task<Result<CreateDietitianCommandResponse>> Handle(
        CreateDietitianCommand request,
        CancellationToken cancellationToken)
    {

        var createdUser = await _authService.RegisterDietitianAsync(new Auth.RegisterDietitian.RegisterDietitianCommand
        {
            Email = request.Email,
            Password = request.Password,
            FirstName = request.FirstName,
            LastName = request.LastName
        });

        var dietitian = await _unitOfWork
            .Dietitians
            .GetByIdAsync(createdUser.Value.UserId);

        return Result<CreateDietitianCommandResponse>.Success(new CreateDietitianCommandResponse
        {
            Id = dietitian.ApplicationUserId,
            FirstName = dietitian.FirstName,
            LastName = dietitian.LastName,
            ApplicationUserId = dietitian.ApplicationUserId,
            CreatedAt = dietitian.CreatedAt
        });
    }
}
