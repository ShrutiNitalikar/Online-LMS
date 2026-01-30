using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_LMS.Models
{
    //public enum UserRole
    //{
    //    Admin = 1,
    //    Mentor = 2,
    //    Student = 3
    //}

    //public class User
    //{
    //    [Key]
    //    public int UserId { get; set; }

    //    // ================= BASIC INFO =================
    //    [Required, MaxLength(100)]
    //    public string FirstName { get; set; } = null!;

    //    [Required, MaxLength(100)]
    //    public string LastName { get; set; } = null!;

    //    [Required, MaxLength(150)]
    //    public string Email { get; set; } = null!;

    //    [Required, MaxLength(100)]
    //    public string Username { get; set; } = null!;

    //    // Optional (don’t force Required unless truly mandatory)
    //    [MaxLength(15)]
    //    public string? MobileNumber { get; set; }

    //    [Required]
    //    public string PasswordHash { get; set; } = null!;

    //    // ================= PROFILE =================
    //    public DateTime? DateOfBirth { get; set; }

    //    [MaxLength(300)]
    //    public string? Address { get; set; }

    //    [MaxLength(500)]
    //    public string? Bio { get; set; }

    //    // Mentor-only (can be NULL for Students)
    //    [MaxLength(200)]
    //    public string? HighestEducation { get; set; }

    //    public UserRole Role { get; set; } = UserRole.Student;

    //    public string? ProfileImageUrl { get; set; }

    //    // ================= AUDIT =================
    //    [Required]
    //    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    //    // ================= OTP =================
    //    [MaxLength(10)]
    //    public string? Otp { get; set; }

    //    public DateTime? OtpExpiry { get; set; }

    //    // ================= RELATIONS =================
    //    public ICollection<CourseFeedback>? Feedbacks { get; set; }
    //}

    public enum UserRole
    {
        Admin = 1,
        Mentor = 2,
        Student = 3
    }

    public class User
    {
        [Key]
        public int UserId { get; set; }

        // ================= BASIC INFO =================
        [Required, MaxLength(100)]
        public string FirstName { get; set; } = null!;

        [Required, MaxLength(100)]
        public string LastName { get; set; } = null!;

        [Required, MaxLength(150)]
        public string Email { get; set; } = null!;

        [Required, MaxLength(100)]
        public string Username { get; set; } = null!;

        // Optional (don’t force Required unless truly mandatory)
        [MaxLength(15)]
        public string? MobileNumber { get; set; }

        [Required]
        public string PasswordHash { get; set; } = null!;

        // ================= PROFILE =================
        public DateTime? DateOfBirth { get; set; }

        [MaxLength(300)]
        public string? Address { get; set; }

        [MaxLength(500)]
        public string? Bio { get; set; }

        // Mentor-only (can be NULL for Students)
        [MaxLength(200)]
        public string? HighestEducation { get; set; }

        public UserRole Role { get; set; } = UserRole.Student;

        public string? ProfileImageUrl { get; set; }

        // ✅ BLOCK (SOFT) — user stays in DB but cannot login
        public bool IsBlocked { get; set; } = false;

        // ================= AUDIT =================
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // ================= OTP =================
        [MaxLength(10)]
        public string? Otp { get; set; }

        public DateTime? OtpExpiry { get; set; }

        // ================= RELATIONS =================
        public ICollection<CourseFeedback>? Feedbacks { get; set; }
    }
}
