using System.ComponentModel.DataAnnotations;

namespace LMS.Api.Models;

public class LectureMaterial
{
    [Key]
    public int MaterialId { get; set; }

    public string Type { get; set; } = null!; // PDF, PPT, LINK
    public string Url { get; set; } = null!;
    public DateTime UploadedOn { get; set; }

    public int TopicId { get; set; }
    public SectionTopic Topic { get; set; } = null!;
}
