using AutoMapper;
using DietManagementSystem.Application.Services;
using MediatR;

namespace DietManagementSystem.Application.Features.Auth.RegisterDietitian;

public class RegisterDietitianCommandHandler : IRequestHandler<RegisterDietitianCommand, Result<RegisterDietitianCommandResponse>>
{
    private readonly IAuthService _authService;
    private readonly ILoggingService _loggingService;
    public RegisterDietitianCommandHandler(IAuthService authService, ILoggingService loggingService)
    {
        _authService = authService;
        _loggingService = loggingService;
    }

    public async Task<Result<RegisterDietitianCommandResponse>> Handle(RegisterDietitianCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _authService.RegisterDietitianAsync(request);

            if (result is null)
            {
                return Result<RegisterDietitianCommandResponse>.Failure("Login failed. Please check your credentials and try again.");
            }

            if (result.IsFailure)
            {
                return Result<RegisterDietitianCommandResponse>.Failure(result.Error);
            }

            return Result<RegisterDietitianCommandResponse>.Success(result.Value);
        }
        catch (Exception e)
        {
            return Result<RegisterDietitianCommandResponse>.Failure($"An error occurred while processing the request: {e.Message}");
        }
    }
}
