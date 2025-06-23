using DietManagementSystem.Domain.Enums;

namespace DietManagementSystem.Application.Common;

public interface IAuthResponse
{
    string Token { get; set; }
    DateTime Expiration { get; set; }
    Guid UserId { get; set; }
    string Email { get; set; }
    UserType UserType { get; set; }
}

