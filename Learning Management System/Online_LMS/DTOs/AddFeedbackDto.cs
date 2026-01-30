using System.ComponentModel.DataAnnotations;

namespace Online_LMS.DTOs
{
    public class AddFeedbackDto
    {
        [Required]
        public int CourseId { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [Required]
        public string Comment { get; set; } = string.Empty;
    }
}
