using System.ComponentModel.DataAnnotations;

namespace LMS.Api.Models;

public class User
{
    [Key]
    public int UserId { get; set; }

    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;
    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? PhNo { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Address { get; set; }
    public string? Bio { get; set; }

    public int RoleId { get; set; }
    public Role Role { get; set; } = null!;

    public string? HighestEducation { get; set; }
    public string? ProfileImg { get; set; }

    public DateTime CreatedAt { get; set; }

    public ICollection<Course> Courses { get; set; } = new List<Course>();
}
