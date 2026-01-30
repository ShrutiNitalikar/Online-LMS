using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_LMS.Data;
using Online_LMS.DTOs;
using Online_LMS.Models;
using System.Security.Claims;

namespace Online_LMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbackController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FeedbackController(AppDbContext context)
        {
            _context = context;
        }

        // ==============================
        // STUDENT: Add Feedback
        // ==============================
        [Authorize(Roles = "Student")]
        [HttpPost]
        public async Task<IActionResult> AddFeedback([FromBody] AddFeedbackDto dto)
        {
            int studentId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var course = await _context.Courses
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CourseId == dto.CourseId);

            if (course == null)
                return NotFound(new { message = "Course not found" });

            bool alreadyGiven = await _context.CourseFeedbacks
                .AnyAsync(f => f.CourseId == dto.CourseId && f.StudentId == studentId && !f.IsDeleted);

            if (alreadyGiven)
                return BadRequest(new { message = "Feedback already submitted" });

            var feedback = new CourseFeedback
            {
                CourseId = dto.CourseId,
                StudentId = studentId,
                Rating = dto.Rating,
                Comment = dto.Comment,
                CreatedAt = DateTime.UtcNow
            };

            _context.CourseFeedbacks.Add(feedback);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Feedback submitted successfully" });
        }

        // ==============================
        // PUBLIC: Get feedback by course (hide deleted)
        // ==============================
        [HttpGet("course/{courseId}")]
        public async Task<IActionResult> GetCourseFeedback(int courseId)
        {
            int? currentUserId = null;

            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                var idStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!string.IsNullOrWhiteSpace(idStr))
                    currentUserId = int.Parse(idStr);
            }

            var feedbacks = await _context.CourseFeedbacks
                .Where(f => f.CourseId == courseId && !f.IsDeleted)
                .Include(f => f.Student)
                .OrderByDescending(f => f.CreatedAt)
                .Select(f => new
                {
                    f.FeedbackId,
                    f.Rating,
                    f.Comment,
                    StudentName = f.Student != null
                        ? f.Student.FirstName + " " + f.Student.LastName
                        : "Deleted User",
                    f.CreatedAt,
                    IsOwnFeedback = currentUserId.HasValue && f.StudentId == currentUserId.Value
                })
                .ToListAsync();

            return Ok(feedbacks);
        }

        // ==============================
        // STUDENT: Update Feedback (only if not deleted)
        // ==============================
        [Authorize(Roles = "Student")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFeedback(int id, UpdateFeedbackDto dto)
        {
            int studentId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var feedback = await _context.CourseFeedbacks
                .FirstOrDefaultAsync(f => f.FeedbackId == id && f.StudentId == studentId && !f.IsDeleted);

            if (feedback == null)
                return NotFound(new { message = "Feedback not found" });

            feedback.Rating = dto.Rating;
            feedback.Comment = dto.Comment;
            feedback.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Feedback updated successfully" });
        }

        // ==============================
        // MENTOR: View feedback for own courses (hide deleted)
        // ==============================
        [Authorize(Roles = "Mentor")]
        [HttpGet("mentor")]
        public async Task<IActionResult> GetMentorCourseFeedback()
        {
            int mentorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var feedbacks = await _context.CourseFeedbacks
                .Include(f => f.Course)
                .Include(f => f.Student)
                .Where(f => f.Course != null && f.Course.MentorId == mentorId && !f.IsDeleted)
                .OrderByDescending(f => f.CreatedAt)
                .Select(f => new
                {
                    f.FeedbackId,
                    CourseTitle = f.Course!.Title,
                    f.Rating,
                    f.Comment,
                    StudentName = f.Student != null
                        ? f.Student.FirstName + " " + f.Student.LastName
                        : "Deleted User",
                    f.CreatedAt,
                    f.IsReported
                })
                .ToListAsync();

            return Ok(feedbacks);
        }

        // ==============================
        // MENTOR: Report Feedback (only if belongs to mentor + not deleted)
        // ==============================
        [Authorize(Roles = "Mentor")]
        [HttpPost("{id}/report")]
        public async Task<IActionResult> ReportFeedback(int id)
        {
            int mentorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var feedback = await _context.CourseFeedbacks
                .Include(f => f.Course)
                .FirstOrDefaultAsync(f => f.FeedbackId == id);

            if (feedback == null || feedback.IsDeleted)
                return NotFound(new { message = "Feedback not found" });

            if (feedback.Course == null || feedback.Course.MentorId != mentorId)
                return Forbid("You can only report feedback for your own courses.");

            if (feedback.IsReported)
                return BadRequest(new { message = "Feedback already reported" });

            feedback.IsReported = true;
            feedback.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Feedback reported successfully", isReported = true });
        }
    }
}
