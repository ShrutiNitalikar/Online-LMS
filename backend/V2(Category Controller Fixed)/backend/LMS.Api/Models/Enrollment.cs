using System.ComponentModel.DataAnnotations;

namespace LMS.Api.Models;

public class Enrollment
{
    [Key]
    public int EnrollmentId { get; set; }

    public int StudentId { get; set; }
    public User Student { get; set; } = null!;

    public int CourseId { get; set; }
    public Course Course { get; set; } = null!;

    public DateTime EnrolledOn { get; set; }
    public int ProgressPercentage { get; set; }
    public bool IsCompleted { get; set; }
}
