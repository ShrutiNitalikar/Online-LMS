using LMS.Api.Data;
using LMS.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Api.Controllers;

[ApiController]
[Route("api/assignments")]
[Authorize(Roles = "Teacher")]// Teacher
public class AssignmentController : ControllerBase
{
    private readonly AppDbContext _context;

    public AssignmentController(AppDbContext context)
    {
        _context = context;
    }

    // 🔹 CREATE ASSIGNMENT (Teacher)
    [HttpPost]
    public IActionResult CreateAssignment(Assignment assignment)
    {
        _context.Assignments.Add(assignment);
        _context.SaveChanges();
        return Ok(assignment);
    }

    // 🔹 GET ALL ASSIGNMENTS (for testing / admin)
    [HttpGet]
    [AllowAnonymous]
    public IActionResult GetAll()
    {
        return Ok(_context.Assignments.ToList());
    }

    // 🔹 GET ASSIGNMENTS BY COURSE
    [HttpGet("course/{courseId}")]
    [AllowAnonymous]
    public IActionResult GetByCourse(int courseId)
    {
        return Ok(
            _context.Assignments
                .Where(a => a.CourseId == courseId)
                .ToList()
        );
    }

    [HttpGet("{assignmentId}/submissions")]
    [Authorize(Roles = "Teacher")] // Teacher
    public IActionResult GetSubmissions(int assignmentId)
    {
        return Ok(
            _context.AssignmentSubmissions
                .Where(s => s.AssignmentId == assignmentId)
                .Select(s => new
                {
                    s.SubmissionId,
                    s.StudentId,
                    s.UploadedFile,
                    s.Status,
                    s.Marks,
                    s.Feedback,
                    s.UploadedOn
                })
                .ToList()
        );
    }

}
