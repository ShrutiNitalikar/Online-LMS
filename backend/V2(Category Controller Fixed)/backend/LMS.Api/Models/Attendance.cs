using System.ComponentModel.DataAnnotations;

namespace LMS.Api.Models;

public class Attendance
{
    [Key]
    public int AttendanceId { get; set; }

    public int StudentId { get; set; }
    public User Student { get; set; } = null!;

    public int TopicId { get; set; }
    public SectionTopic Topic { get; set; } = null!;

    public DateTime AttendanceDate { get; set; }
    public string Status { get; set; } = null!; // Present, Absent
}
