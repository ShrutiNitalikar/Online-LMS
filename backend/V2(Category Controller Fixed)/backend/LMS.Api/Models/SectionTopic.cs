using System.ComponentModel.DataAnnotations;

namespace LMS.Api.Models;

public class SectionTopic
{
    [Key]
    public int TopicId { get; set; }

    public string TopicName { get; set; } = null!;
    public string? TopicDescription { get; set; }

    public int SectionId { get; set; }
    public CourseSection Section { get; set; } = null!;

    public ICollection<LectureMaterial> Materials { get; set; } = new List<LectureMaterial>();
}
