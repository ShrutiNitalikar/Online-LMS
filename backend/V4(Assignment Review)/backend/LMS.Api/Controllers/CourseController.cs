using LMS.Api.Data;
using LMS.Api.Dtos;
using LMS.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

[ApiController]
[Route("api/courses")]
public class CourseController : ControllerBase
{
    private readonly AppDbContext _context;

    public CourseController(AppDbContext context)
    {
        _context = context;
    }

    // 🔹 Create course (Teacher only)
    [HttpPost]
    [Authorize(Roles = "2")]
    public IActionResult CreateCourse(CourseCreateDto dto)
    {
        var course = new Course
        {
            Title = dto.Title,
            Description = dto.Description,
            TeacherId = dto.TeacherId,
            CategoryId = dto.CategoryId,
            PublishDate = DateTime.UtcNow,
            Status = "Draft"
        };

        _context.Courses.Add(course);
        _context.SaveChanges();

        return Ok(course);
    }


    // 🔹 Publish course (Teacher/Admin)
    [HttpPut("{id}/publish")]
    [Authorize(Roles = "2,3")]
    public IActionResult PublishCourse(int id)
    {
        var course = _context.Courses.Find(id);
        if (course == null) return NotFound();

        course.Status = "Published";
        course.PublishDate = DateTime.UtcNow;
        _context.SaveChanges();

        return Ok("Course published");
    }

    // 🔹 View published courses (Everyone)
    [HttpGet("published")]
    public IActionResult GetPublishedCourses()
    {
        return Ok(
            _context.Courses
                .Where(c => c.Status == "Published")
                .ToList()
        );
    }
}
