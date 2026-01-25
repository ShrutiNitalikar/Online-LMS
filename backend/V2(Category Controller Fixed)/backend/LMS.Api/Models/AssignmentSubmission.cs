using System.ComponentModel.DataAnnotations;

namespace LMS.Api.Models;

public class AssignmentSubmission
{
    [Key] // EXPLICIT PRIMARY KEY
    public int SubmissionId { get; set; }

    public int AssignmentId { get; set; }
    public Assignment Assignment { get; set; } = null!;

    public int StudentId { get; set; }
    public User Student { get; set; } = null!;

    public string? Status { get; set; } // Submitted, Reviewed, Late
    public string? UploadedFile { get; set; }
    public DateTime UploadedOn { get; set; }

    public int? Marks { get; set; }
    public string? Feedback { get; set; }
    public DateTime? ReviewedOn { get; set; }
}
