namespace LMS.Api.Dtos;

public class TopicCreateDto
{
    public int SectionId { get; set; }
    public string TopicName { get; set; } = null!;
    public string? TopicDescription { get; set; }
}
