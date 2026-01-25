using LMS.Api.Data;
using LMS.Api.Dtos;
using LMS.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/sections")]
[Authorize(Roles = "2")]
public class SectionController : ControllerBase
{
    private readonly AppDbContext _context;

    public SectionController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public IActionResult AddSection(SectionCreateDto dto)
    {
        var courseExists = _context.Courses.Any(c => c.CourseId == dto.CourseId);
        if (!courseExists)
            return NotFound("Course not found");

        var section = new CourseSection
        {
            CourseId = dto.CourseId,
            SectionName = dto.SectionName,
            SectionDescription = dto.SectionDescription
        };

        _context.CourseSections.Add(section);
        _context.SaveChanges();

        return Ok(section);
    }
}
