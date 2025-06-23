using DietManagementSystem.Domain.Entities;
using DietManagementSystem.Domain.Entities.Common;
using DietManagementSystem.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace DietManagementSystem.Domain.Identity;

public class ApplicationUser : IdentityUser<Guid>, IAuditable
{
    public string CreatedBy { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public string? UpdatedBy { get; set; } = null!;
    public DateTime? UpdatedAt { get; set; }

    public UserType UserType { get; set; }

    public Client? Client { get; set; }
    public Dietitian? Dietitian { get; set; }
}
