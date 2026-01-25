using LMS.Api.Data;
using LMS.Api.Dtos;
using LMS.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/topics")]
[Authorize(Roles = "2")]
public class TopicController : ControllerBase
{
    private readonly AppDbContext _context;

    public TopicController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public IActionResult AddTopic(TopicCreateDto dto)
    {
        var sectionExists = _context.CourseSections.Any(s => s.SectionId == dto.SectionId);
        if (!sectionExists)
            return NotFound("Section not found");

        var topic = new SectionTopic
        {
            SectionId = dto.SectionId,
            TopicName = dto.TopicName,
            TopicDescription = dto.TopicDescription
        };

        _context.SectionTopics.Add(topic);
        _context.SaveChanges();

        return Ok(topic);
    }
}
