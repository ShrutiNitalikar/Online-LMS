namespace LMS.Api.Dtos;

public class SectionCreateDto
{
    public int CourseId { get; set; }
    public string SectionName { get; set; } = null!;
    public string? SectionDescription { get; set; }
}
