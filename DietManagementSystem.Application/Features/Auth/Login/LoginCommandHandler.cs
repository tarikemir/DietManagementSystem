using AutoMapper;
using DietManagementSystem.Application.Services;
using MediatR;

namespace DietManagementSystem.Application.Features.Auth.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginCommandResponse>>
{
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;
    private readonly ILoggingService _loggingService;
    public LoginCommandHandler(IAuthService authService, IMapper mapper, ILoggingService loggingService)
    {
        _authService = authService;
        _mapper = mapper;
        _loggingService = loggingService;
    }
    public async Task<Result<LoginCommandResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _authService.LoginAsync(request);

            if (result is null)
            {
                return Result<LoginCommandResponse>.Failure("Login failed. Please check your credentials and try again.");
            }

            if (result.IsFailure)
            {
                return Result<LoginCommandResponse>.Failure(result.Error);
            }

            return Result<LoginCommandResponse>.Success(result.Value);
        }
        catch( Exception e)
        {
            return Result<LoginCommandResponse>.Failure($"An error occurred while processing the request: {e.Message}");
        }
        
    }
}
