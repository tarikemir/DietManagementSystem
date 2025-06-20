using DietManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace DietManagementSystem.Domain.Identity;

public class ApplicationUser : IdentityUser<Guid>
{
    public Client? Client { get; set; }
    public Dietitian? Dietitian { get; set; }
}
