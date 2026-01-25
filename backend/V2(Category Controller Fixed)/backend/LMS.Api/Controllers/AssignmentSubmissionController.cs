using LMS.Api.Data;
using LMS.Api.Models;
using LMS.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LMS.Api.Controllers;

[ApiController]
[Route("api/submissions")]
[Authorize(Roles = "1")]
public class AssignmentSubmissionController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly FileStorageService _fileService;

    public AssignmentSubmissionController(
        AppDbContext context,
        FileStorageService fileService)
    {
        _context = context;
        _fileService = fileService;
    }

    [HttpPost("{assignmentId}")]
    public async Task<IActionResult> SubmitAssignment(
        int assignmentId,
        IFormFile file)
    {
        int studentId = int.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var fileUrl = await _fileService.UploadAsync(file, "submissions");

        var submission = new AssignmentSubmission
        {
            AssignmentId = assignmentId,
            StudentId = studentId,
            UploadedFile = fileUrl,
            UploadedOn = DateTime.UtcNow,
            Status = "Submitted"
        };

        _context.AssignmentSubmissions.Add(submission);
        _context.SaveChanges();

        return Ok("Assignment submitted");
    }
}
