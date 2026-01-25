using System.ComponentModel.DataAnnotations;

namespace LMS.Api.Models;

public class Category
{
    [Key]
    public int CategoryId { get; set; }

    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public ICollection<Course> Courses { get; set; } = new List<Course>();
}
