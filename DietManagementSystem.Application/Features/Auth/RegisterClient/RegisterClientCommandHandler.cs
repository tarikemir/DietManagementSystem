using AutoMapper;
using DietManagementSystem.Application.Services;
using MediatR;

namespace DietManagementSystem.Application.Features.Auth.RegisterClient;

public class RegisterClientCommandHandler : IRequestHandler<RegisterClientCommand, Result<RegisterClientCommandResponse>>
{
    private readonly IAuthService _authService;
    private readonly ILoggingService _loggingService;
    public RegisterClientCommandHandler(IAuthService authService, ILoggingService loggingService)
    {
        _authService = authService;
        _loggingService = loggingService;
    }

    public async Task<Result<RegisterClientCommandResponse>> Handle(RegisterClientCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _authService.RegisterClientAsync(request);

            if (result is null)
            {
                return Result<RegisterClientCommandResponse>.Failure("Login failed. Please check your credentials and try again.");
            }

            if (result.IsFailure)
            {
                return Result<RegisterClientCommandResponse>.Failure(result.Error);
            }

            return Result<RegisterClientCommandResponse>.Success(result.Value);
        }
        catch (Exception e)
        {
            return Result<RegisterClientCommandResponse>.Failure($"An error occurred while processing the request: {e.Message}");
        }
    }
}
