using System.ComponentModel.DataAnnotations;

namespace LMS.Api.Models;

public class Course
{
    [Key]
    public int CourseId { get; set; }

    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime? PublishDate { get; set; }
    public string? Note { get; set; }
    public string? Thumbnail { get; set; }
    public string Status { get; set; } = "Draft";

    public int TeacherId { get; set; }
    public User Teacher { get; set; } = null!;

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public ICollection<CourseSection> Sections { get; set; } = new List<CourseSection>();
}
