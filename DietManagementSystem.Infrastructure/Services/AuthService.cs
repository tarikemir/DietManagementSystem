using DietManagementSystem.Application.Services;
using DietManagementSystem.Domain.Entities;
using DietManagementSystem.Domain.Enums;
using DietManagementSystem.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using DietManagementSystem.Application.Settings;
using Microsoft.Extensions.Options;
using DietManagementSystem.Application.Features.Auth.Login;
using DietManagementSystem.Application.Features.Auth.RegisterClient;
using DietManagementSystem.Application.Features.Auth.RegisterDietitian;
using DietManagementSystem.Application.Common;
using System.Data;
using Microsoft.EntityFrameworkCore;
using DietManagementSystem.Infrastructure.Repositories;

namespace DietManagementSystem.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly JwtSettings _jwtSettings;
    private readonly ILoggingService _loggingService;

    public AuthService(UserManager<ApplicationUser> userManager, IOptions<JwtSettings> jwtSettings, ILoggingService loggingService, IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _jwtSettings = jwtSettings.Value;
        _loggingService = loggingService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<LoginCommandResponse>> LoginAsync(LoginCommand request)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                _loggingService.LogWarning("Login failed - User not found for email: {Email}", request.Email);
                return Result<LoginCommandResponse>.Failure("User not found");
            }

            var result = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!result)
            {
                _loggingService.LogWarning("Login failed - Invalid credentials for email: {Email}", request.Email);
                return Result<LoginCommandResponse>.Failure("Invalid credentials");
            }

            return Result<LoginCommandResponse>.Success(await GenerateAuthResponse<LoginCommandResponse>(user));
        }
        catch (Exception ex)
        {
            _loggingService.LogError(ex, "Error during login for email: {Email}", request.Email);
            return Result<LoginCommandResponse>.Failure($"Login failed: {ex.Message}");
        }
    }

    public async Task<Result<RegisterClientCommandResponse>> RegisterClientAsync(RegisterClientCommand request)
    {
        try
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);

            if (existingUser != null)
            {
                _loggingService.LogWarning("Client registration failed - Email already in use: {Email}", request.Email);
                return Result<RegisterClientCommandResponse>.Failure("Email already in use");
            }

            var newUser = new ApplicationUser
            {
                Email = request.Email,
                UserName = request.Email,
                UserType = UserType.Client,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System"
            };

            var createdUser = await _userManager.CreateAsync(newUser, request.Password);

            if (!createdUser.Succeeded)
            {
                var errors = string.Join(", ", createdUser.Errors.Select(x => x.Description));
                _loggingService.LogError("Client registration failed - User creation errors: {Errors}", errors);
                return Result<RegisterClientCommandResponse>.Failure(errors);
            }

            await _userManager.AddToRoleAsync(newUser, UserType.Client.ToString());

            var client = new Client
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                InitialWeight = request.InitialWeight,
                DietitianId = request.DietitianId,// Requestteki DietitianId dietitianIdnin applicationidsi
                ApplicationUserId = newUser.Id, // newUser client için oluşturulan ApplicationUser 
                CreatedBy = "System"
            };

            newUser.Client = client;

            var updateResult = await _userManager.UpdateAsync(newUser);
            if (!updateResult.Succeeded)
            {
                var errors = string.Join(", ", updateResult.Errors.Select(x => x.Description));
                _loggingService.LogError("Client registration failed - User update errors: {Errors}", errors);
                return Result<RegisterClientCommandResponse>.Failure(errors);
            }

            return Result<RegisterClientCommandResponse>.Success(await GenerateAuthResponse<RegisterClientCommandResponse>(newUser));
        }
        catch (Exception ex)
        {
            _loggingService.LogError(ex, "Error during client registration for email: {Email}", request.Email);
            return Result<RegisterClientCommandResponse>.Failure($"Registration failed: {ex.Message}");
        }
    }

    public async Task<Result<RegisterDietitianCommandResponse>> RegisterDietitianAsync(RegisterDietitianCommand request)
    {
        try
        {

            var existingUser = await _userManager.FindByEmailAsync(request.Email);

            if (existingUser != null)
            {
                _loggingService.LogWarning("Dietitian registration failed - Email already in use: {Email}", request.Email);
                return Result<RegisterDietitianCommandResponse>.Failure("Email already in use");
            }

            var newUser = new ApplicationUser
            {
                Email = request.Email,
                UserName = request.Email,
                UserType = UserType.Dietitian,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System"
            };

            var createdUser = await _userManager.CreateAsync(newUser, request.Password);

            if (!createdUser.Succeeded)
            {
                var errors = string.Join(", ", createdUser.Errors.Select(x => x.Description));
                _loggingService.LogError("Dietitian registration failed - User creation errors: {Errors}", errors);
                return Result<RegisterDietitianCommandResponse>.Failure(errors);
            }

            await _userManager.AddToRoleAsync(newUser, UserType.Dietitian.ToString());

            var dietitian = new Dietitian
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                ApplicationUserId = newUser.Id,
                CreatedBy = $"{request.FirstName}-{request.LastName}"
            };

            newUser.Dietitian = dietitian;

            var updateResult = await _userManager.UpdateAsync(newUser);
            if (!updateResult.Succeeded)
            {
                var errors = string.Join(", ", updateResult.Errors.Select(x => x.Description));
                _loggingService.LogError("Dietitian registration failed - User update errors: {Errors}", errors);
                return Result<RegisterDietitianCommandResponse>.Failure(errors);
            }

            return Result<RegisterDietitianCommandResponse>.Success(await GenerateAuthResponse<RegisterDietitianCommandResponse>(newUser));
        }
        catch (Exception ex)
        {
            _loggingService.LogError(ex, "Error during dietitian registration for email: {Email}", request.Email);
            return Result<RegisterDietitianCommandResponse>.Failure($"Registration failed: {ex.Message}");
        }
    }

    private async Task<T> GenerateAuthResponse<T>(ApplicationUser user) where T : IAuthResponse, new()
    {
        try
        {

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

            user.Client = await _unitOfWork.Clients.FirstOrDefaultAsync(c => c.ApplicationUserId == user.Id);
            user.Dietitian = await _unitOfWork.Dietitians.FirstOrDefaultAsync(d => d.ApplicationUserId == user.Id);

            Guid arrangedId;
            if (user.Client != null)
            {
                arrangedId = user.Client.Id; // Client ID'ye denk geliyor.
            }
            else if (user.Dietitian != null)
            {
                arrangedId = user.Dietitian.Id; // Dietitian ID'ye denk geliyor.
            }
            else
            {
                arrangedId = user.Id; // Eğer Client veya Dietitian yoksa, ApplicationUser ID'si kullanılıyor.
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Role, user.UserType.ToString()),
                        new Claim(JwtRegisteredClaimNames.Sub, arrangedId.ToString()),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Email, user.Email),
                        new Claim("userid", user.Id.ToString()),
                        new Claim("userType", user.UserType.ToString())
                    }
                ),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);


            

            return new()
            {
                Token = tokenHandler.WriteToken(token),
                Expiration = token.ValidTo
            };
        }
        catch (Exception ex)
        {
            _loggingService.LogError(ex, "Error generating JWT token for user ID: {UserId}", user.Id);
            throw;
        }
    }
}
