namespace LMS.Api.Dtos;

public class CourseCreateDto
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int TeacherId { get; set; }
    public int CategoryId { get; set; }
}
