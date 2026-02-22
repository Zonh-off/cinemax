using Microsoft.AspNetCore.Identity;

namespace Core.Entities;

public class AppUser : IdentityUser
{
    public string? FullName { get; set; }
    public DateTime? DateOfBirth { get; set; }
}