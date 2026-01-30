using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_LMS.Models
{
    
    public class CourseFeedback
    {
        [Key]
        public int FeedbackId { get; set; }

        // =========================
        // Foreign Keys
        // =========================

        [Required]
        [ForeignKey(nameof(Course))]
        public int CourseId { get; set; }
        public Course? Course { get; set; }

        [Required]
        [ForeignKey(nameof(Student))]
        public int StudentId { get; set; }
        public User? Student { get; set; }

        // =========================
        // Feedback Content
        // =========================

        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }   // ⭐ 1–5

        [Required]
        [MaxLength(1000, ErrorMessage = "Comment cannot exceed 1000 characters")]
        public string Comment { get; set; } = string.Empty;

        // =========================
        // Moderation & Control
        // =========================

        // Mentor can report feedback
        public bool IsReported { get; set; } = false;

        // Soft delete (Admin use)
        public bool IsDeleted { get; set; } = false;

        // Track edits
        public DateTime? UpdatedAt { get; set; }

        // =========================
        // Audit
        // =========================

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
