using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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

    [JsonIgnore]
    public Course? Course { get; set; }   // ✅ MAKE NULLABLE

    [JsonIgnore]
    public ICollection<AssignmentSubmission> Submissions { get; set; }
        = new List<AssignmentSubmission>();
}
