using System.ComponentModel.DataAnnotations;

namespace LMS.Api.Models;

public class Assignment
{
    [Key]
    public int AssignmentId { get; set; }

    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public int? MaxMarks { get; set; }

    public int CourseId { get; set; }
    public Course Course { get; set; } = null!;

    public ICollection<AssignmentSubmission> Submissions { get; set; } = new List<AssignmentSubmission>();
}
