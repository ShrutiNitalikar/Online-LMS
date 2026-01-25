using LMS.Api.Data;
using LMS.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

[ApiController]
[Route("api/enrollment")]
[Authorize(Roles = "1")]
public class EnrollmentController : ControllerBase
{
    private readonly AppDbContext _context;

    public EnrollmentController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("{courseId}")]
    public IActionResult Enroll(int courseId)
    {
        int studentId = int.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier));

        if (_context.Enrollments.Any(e =>
            e.CourseId == courseId && e.StudentId == studentId))
        {
            return BadRequest("Already enrolled");
        }

        var enrollment = new Enrollment
        {
            CourseId = courseId,
            StudentId = studentId
        };

        _context.Enrollments.Add(enrollment);
        _context.SaveChanges();

        return Ok("Enrolled successfully");
    }
}
