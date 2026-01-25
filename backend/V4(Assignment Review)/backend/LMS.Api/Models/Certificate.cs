using System.ComponentModel.DataAnnotations;

namespace LMS.Api.Models;

public class Certificate
{
    [Key]
    public int CertificateId { get; set; }

    public string CertificateNo { get; set; } = null!;

    public int StudentId { get; set; }
    public User Student { get; set; } = null!;

    public int CourseId { get; set; }
    public Course Course { get; set; } = null!;

    public DateTime CompletedOn { get; set; }
    public string? CertificateUrl { get; set; }
}
