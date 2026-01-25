using System.ComponentModel.DataAnnotations;

namespace LMS.Api.Models;

public class Role
{
    [Key]
    public int RoleId { get; set; }
    public string RoleName { get; set; } = null!;

    public ICollection<User> Users { get; set; } = new List<User>();
}
