using LMS.Api.Data;
using LMS.Api.Dtos;
using LMS.Api.Models;
using LMS.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LMS.Api.Controllers;

[ApiController]
[Route("api/submissions")]
[Authorize] // authenticated users only
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

    // 🔹 Submit assignment (Student)
    [HttpPost("{assignmentId}")]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> SubmitAssignment(
        int assignmentId,
        [FromForm] IFormFile file)
    {
        // Validate file
        if (file == null || file.Length == 0)
            return BadRequest("File is required");

        // Get student ID from JWT
        var studentIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (studentIdClaim == null)
            return Unauthorized("Invalid token");

        int studentId = int.Parse(studentIdClaim.Value);

        // Upload file
        var fileUrl = await _fileService.UploadAsync(file, "submissions");

        // Save submission
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

        return Ok(submission);
    }

    [HttpPut("{submissionId}/review")]
    [Authorize(Roles = "Teacher")]
    public IActionResult ReviewSubmission(
    int submissionId,
    [FromBody] AssignmentReviewDto dto)
    {
        var submission = _context.AssignmentSubmissions
            .FirstOrDefault(s => s.SubmissionId == submissionId);

        if (submission == null)
            return NotFound("Submission not found");

        submission.Marks = dto.Marks;
        submission.Feedback = dto.Feedback;
        submission.Status = "Reviewed";
        submission.ReviewedOn = DateTime.UtcNow;

        _context.SaveChanges();

        return Ok(submission);
    }

}
