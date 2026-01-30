namespace Online_LMS.DTOs
{
    public class UpdateFeedbackDto
    {
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
    }
}
