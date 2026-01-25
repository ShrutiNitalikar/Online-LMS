using System.ComponentModel.DataAnnotations;

namespace LMS.Api.Models;

public class CourseSection
{
    [Key] //  Explicit primary key
    public int SectionId { get; set; }

    public string SectionName { get; set; } = null!;
    public string? SectionDescription { get; set; }

    public int CourseId { get; set; }
    public Course Course { get; set; } = null!;

    public ICollection<SectionTopic> Topics { get; set; } = new List<SectionTopic>();
}
