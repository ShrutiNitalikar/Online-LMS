using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_LMS.Data;

namespace Online_LMS.Controllers
{
    [ApiController]
    [Route("api/admin/feedback")]
    [Authorize(Roles = "Admin")]
    public class AdminFeedbackController : ControllerBase
    {
        private readonly AppDbContext _db;

        public AdminFeedbackController(AppDbContext db)
        {
            _db = db;
        }

        // ✅ All feedback (includes deleted for admin view)
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var list = await _db.CourseFeedbacks
                .Include(f => f.Course)
                .Include(f => f.Student)
                .OrderByDescending(f => f.CreatedAt)
                .Select(f => new
                {
                    f.FeedbackId,
                    f.CourseId,
                    CourseTitle = f.Course != null ? f.Course.Title : "Deleted Course",

                    f.StudentId,
                    StudentName = f.Student != null ? (f.Student.FirstName + " " + f.Student.LastName) : "Deleted User",
                    StudentUsername = f.Student != null ? f.Student.Username : "",
                    StudentIsBlocked = f.Student != null ? f.Student.IsBlocked : false,

                    f.Rating,
                    f.Comment,
                    f.IsReported,
                    f.IsDeleted,
                    f.CreatedAt,
                    f.UpdatedAt
                })
                .ToListAsync();

            return Ok(list);
        }

        // ✅ Reported feedback only (not deleted)
        [HttpGet("reported")]
        public async Task<IActionResult> GetReported()
        {
            var list = await _db.CourseFeedbacks
                .Include(f => f.Course)
                .Include(f => f.Student)
                .Where(f => f.IsReported && !f.IsDeleted)
                .OrderByDescending(f => f.CreatedAt)
                .Select(f => new
                {
                    f.FeedbackId,
                    f.CourseId,
                    CourseTitle = f.Course != null ? f.Course.Title : "Deleted Course",

                    f.StudentId,
                    StudentName = f.Student != null ? (f.Student.FirstName + " " + f.Student.LastName) : "Deleted User",
                    StudentUsername = f.Student != null ? f.Student.Username : "",
                    StudentIsBlocked = f.Student != null ? f.Student.IsBlocked : false,

                    f.Rating,
                    f.Comment,
                    f.IsReported,
                    f.IsDeleted,
                    f.CreatedAt,
                    f.UpdatedAt
                })
                .ToListAsync();

            return Ok(list);
        }

        // ✅ Resolve Report (unreport)
        [HttpPut("{id}/resolve")]
        public async Task<IActionResult> ResolveReport(int id)
        {
            var feedback = await _db.CourseFeedbacks.FirstOrDefaultAsync(f => f.FeedbackId == id);
            if (feedback == null) return NotFound(new { message = "Feedback not found" });

            feedback.IsReported = false;
            feedback.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return Ok(new { message = "Report resolved successfully." });
        }

        // ✅ Soft Delete Feedback
        [HttpPut("{id}/delete")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            var feedback = await _db.CourseFeedbacks.FirstOrDefaultAsync(f => f.FeedbackId == id);
            if (feedback == null) return NotFound(new { message = "Feedback not found" });

            feedback.IsDeleted = true;
            feedback.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return Ok(new { message = "Feedback deleted (soft) successfully." });
        }

        // ✅ Restore feedback (optional)
        [HttpPut("{id}/restore")]
        public async Task<IActionResult> Restore(int id)
        {
            var feedback = await _db.CourseFeedbacks.FirstOrDefaultAsync(f => f.FeedbackId == id);
            if (feedback == null) return NotFound(new { message = "Feedback not found" });

            feedback.IsDeleted = false;
            feedback.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return Ok(new { message = "Feedback restored successfully." });
        }
    }
}
