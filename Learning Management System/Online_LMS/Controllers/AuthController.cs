//using BCrypt.Net;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Online_LMS.Data;
//using Online_LMS.DTOs;
//using Online_LMS.Helpers;
//using Online_LMS.Models;
//using Online_LMS.Services;

//namespace Online_LMS.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class AuthController : ControllerBase
//    {
//        private readonly AppDbContext _db;
//        private readonly IConfiguration _config;
//        private readonly EmailService _emailService;

//        public AuthController(
//            AppDbContext db,
//            IConfiguration config,
//            EmailService emailService)
//        {
//            _db = db;
//            _config = config;
//            _emailService = emailService;
//        }

//        // ================= REGISTER =================
//        [HttpPost("register")]
//        public async Task<IActionResult> Register(RegisterDto dto)
//        {
//            if (await _db.Users.AnyAsync(x =>
//                x.Email == dto.Email || x.Username == dto.Username))
//                return BadRequest("Email or Username already exists.");

//            var age = CalculateAge(dto.DateOfBirth);

//            // ✅ ENUM COMPARISON (FIX)
//            if (dto.Role == UserRole.Student && age < 10)
//            {
//                return BadRequest("Student must be at least 10 years old.");
//            }

//            if (dto.Role == UserRole.Mentor && age < 21)
//            {
//                return BadRequest("Mentor must be at least 21 years old.");
//            }


//            var user = new User
//            {
//                FirstName = dto.FirstName,
//                LastName = dto.LastName,
//                Email = dto.Email,
//                Username = dto.Username,
//                MobileNumber = dto.MobileNumber,
//                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
//                DateOfBirth = dto.DateOfBirth,
//                Address = dto.Address,
//                Bio = dto.Bio,
//                Role = dto.Role,
//                HighestEducation = dto.Role == UserRole.Mentor ? dto.HighestEducation : ""
//            };

//            _db.Users.Add(user);
//            await _db.SaveChangesAsync();

//            return Ok("Registered Successfully.");
//        }

//        private int CalculateAge(DateTime? dateOfBirth)
//        {
//            throw new NotImplementedException();
//        }

//        // ================= LOGIN =================
//        [HttpPost("login")]
//        public async Task<IActionResult> Login(LoginDto dto)
//        {
//            var user = await _db.Users.FirstOrDefaultAsync(x => x.Username == dto.Username);
//            if (user == null)
//                return Unauthorized("Invalid username.");

//            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
//                return Unauthorized("Invalid password.");

//            var token = JwtHelper.GenerateToken(_config, user);

//            return Ok(new
//            {
//                token,
//                role = user.Role.ToString(),
//                userId = user.UserId,
//                username = user.Username
//            });
//        }

//        // ================= FORGOT PASSWORD =================
//        [HttpPost("forgot-password")]
//        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto dto)
//        {
//            var user = await _db.Users.FirstOrDefaultAsync(x => x.Email == dto.Email);
//            if (user == null)
//                return BadRequest("Email not registered.");

//            var otp = new Random().Next(100000, 999999).ToString();

//            user.Otp = otp;
//            user.OtpExpiry = DateTime.UtcNow.AddMinutes(5);
//            await _db.SaveChangesAsync();

//            await _emailService.SendEmailAsync(
//                dto.Email,
//                "Your LMS OTP Code",
//                $"Your OTP is {otp}. It is valid for 5 minutes."
//            );

//            return Ok("OTP sent to registered email.");
//        }

//        // ================= VERIFY OTP =================
//        [HttpPost("verify-otp")]
//        public async Task<IActionResult> VerifyOtp(VerifyOtpDto dto)
//        {
//            var user = await _db.Users.FirstOrDefaultAsync(x => x.Email == dto.Email);
//            if (user == null)
//                return BadRequest("Invalid email.");

//            if (user.Otp != dto.Otp)
//                return BadRequest("Invalid OTP.");

//            if (user.OtpExpiry < DateTime.UtcNow)
//                return BadRequest("OTP expired.");

//            // mark OTP as used
//            user.Otp = null;
//            user.OtpExpiry = null;
//            await _db.SaveChangesAsync();

//            return Ok("OTP verified successfully.");
//        }

//        // ================= RESET PASSWORD =================
//        [HttpPost("reset-password")]
//        public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
//        {
//            var user = await _db.Users.FirstOrDefaultAsync(x => x.Email == dto.Email);
//            if (user == null)
//                return NotFound("User not found.");

//            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
//            await _db.SaveChangesAsync();

//            return Ok("Password reset successful.");
//        }
//    }
//}

//using BCrypt.Net;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Online_LMS.Data;
//using Online_LMS.DTOs;
//using Online_LMS.Helpers;
//using Online_LMS.Models;
//using Online_LMS.Services;

//namespace Online_LMS.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class AuthController : ControllerBase
//    {
//        private readonly AppDbContext _db;
//        private readonly IConfiguration _config;
//        private readonly EmailService _emailService;

//        public AuthController(AppDbContext db, IConfiguration config, EmailService emailService)
//        {
//            _db = db;
//            _config = config;
//            _emailService = emailService;
//        }

//        // ================= REGISTER =================
//        [HttpPost("register")]
//        public async Task<IActionResult> Register(RegisterDto dto)
//        {
//            // Check duplicate email or username
//            if (await _db.Users.AnyAsync(x => x.Email == dto.Email || x.Username == dto.Username))
//                return BadRequest("Email or Username already exists.");

//            // Age validation
//            if (!dto.DateOfBirth.HasValue)
//                return BadRequest("Date of Birth is required.");

//            var age = CalculateAge(dto.DateOfBirth.Value);

//            if (dto.Role == UserRole.Student && age < 10)
//                return BadRequest("Student must be at least 10 years old.");

//            if (dto.Role == UserRole.Mentor && age < 21)
//                return BadRequest("Mentor must be at least 21 years old.");

//            // Ensure HighestEducation is empty string for non-mentors
//            var highestEducation = dto.Role == UserRole.Mentor ? dto.HighestEducation?.Trim() ?? "" : "";

//            var user = new User
//            {
//                FirstName = dto.FirstName,
//                LastName = dto.LastName,
//                Email = dto.Email,
//                Username = dto.Username,
//                MobileNumber = dto.MobileNumber,
//                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
//                DateOfBirth = dto.DateOfBirth.Value,
//                Address = dto.Address,
//                Bio = dto.Bio,
//                Role = dto.Role,
//                HighestEducation = highestEducation
//            };

//            try
//            {
//                _db.Users.Add(user);
//                await _db.SaveChangesAsync();
//                return Ok("Registered successfully.");
//            }
//            catch (DbUpdateException ex)
//            {
//                // Log exception if needed
//                return StatusCode(500, "Database error: " + ex.InnerException?.Message);
//            }
//        }

//        // ================= LOGIN =================
//        [HttpPost("login")]
//        public async Task<IActionResult> Login(LoginDto dto)
//        {
//            var user = await _db.Users.FirstOrDefaultAsync(x => x.Username == dto.Username);
//            if (user == null) return Unauthorized("Invalid username.");

//            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
//                return Unauthorized("Invalid password.");

//            var token = JwtHelper.GenerateToken(_config, user);

//            return Ok(new
//            {
//                token,
//                role = user.Role.ToString(),
//                userId = user.UserId,
//                username = user.Username
//            });
//        }

//        // ================= FORGOT PASSWORD =================
//        [HttpPost("forgot-password")]
//        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto dto)
//        {
//            var user = await _db.Users.FirstOrDefaultAsync(x => x.Email == dto.Email);
//            if (user == null) return BadRequest("Email not registered.");

//            var otp = new Random().Next(100000, 999999).ToString();
//            user.Otp = otp;
//            user.OtpExpiry = DateTime.UtcNow.AddMinutes(5);
//            await _db.SaveChangesAsync();

//            await _emailService.SendEmailAsync(dto.Email, "Your LMS OTP Code", $"Your OTP is {otp}. It is valid for 5 minutes.");

//            return Ok("OTP sent to registered email.");
//        }

//        // ================= VERIFY OTP =================
//        [HttpPost("verify-otp")]
//        public async Task<IActionResult> VerifyOtp(VerifyOtpDto dto)
//        {
//            var user = await _db.Users.FirstOrDefaultAsync(x => x.Email == dto.Email);
//            if (user == null) return BadRequest("Invalid email.");
//            if (user.Otp != dto.Otp) return BadRequest("Invalid OTP.");
//            if (user.OtpExpiry < DateTime.UtcNow) return BadRequest("OTP expired.");

//            user.Otp = null;
//            user.OtpExpiry = null;
//            await _db.SaveChangesAsync();

//            return Ok("OTP verified successfully.");
//        }

//        // ================= RESET PASSWORD =================
//        [HttpPost("reset-password")]
//        public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
//        {
//            var user = await _db.Users.FirstOrDefaultAsync(x => x.Email == dto.Email);
//            if (user == null) return NotFound("User not found.");

//            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
//            await _db.SaveChangesAsync();

//            return Ok("Password reset successful.");
//        }

//        // ================= HELPER =================
//        private int CalculateAge(DateTime dateOfBirth)
//        {
//            var today = DateTime.Today;
//            var age = today.Year - dateOfBirth.Year;
//            if (dateOfBirth.Date > today.AddYears(-age)) age--;
//            return age;
//        }
//    }
//}

using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_LMS.Data;
using Online_LMS.DTOs;
using Online_LMS.Helpers;
using Online_LMS.Models;
using Online_LMS.Services;
using System.Security.Cryptography;

namespace Online_LMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _config;
        private readonly EmailService _emailService;

        public AuthController(
            AppDbContext db,
            IConfiguration config,
            EmailService emailService)
        {
            _db = db;
            _config = config;
            _emailService = emailService;
        }

        // ================= REGISTER =================
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (dto.DateOfBirth == null)
                return BadRequest("Date of Birth is required.");

            if (dto.DateOfBirth > DateTime.UtcNow.Date)
                return BadRequest("Invalid Date of Birth.");

            if (await _db.Users.AnyAsync(x =>
                x.Email == dto.Email || x.Username == dto.Username))
                return BadRequest("Email or Username already exists.");

            var age = CalculateAge(dto.DateOfBirth.Value);

            if (dto.Role == UserRole.Student && age < 10)
                return BadRequest("Student must be at least 10 years old.");

            if (dto.Role == UserRole.Mentor && age < 21)
                return BadRequest("Mentor must be at least 21 years old.");

            if (dto.Role == UserRole.Mentor &&
                string.IsNullOrWhiteSpace(dto.HighestEducation))
                return BadRequest("Highest education is required for mentors.");

            var user = new User
            {
                FirstName = dto.FirstName.Trim(),
                LastName = dto.LastName.Trim(),
                Email = dto.Email.Trim().ToLower(),
                Username = dto.Username.Trim(),
                MobileNumber = dto.MobileNumber,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),

                // ✅ Safe DOB handling
                DateOfBirth = DateTime.SpecifyKind(
                    dto.DateOfBirth.Value,
                    DateTimeKind.Utc
                ),

                Address = dto.Address,
                Bio = dto.Bio,
                Role = dto.Role,
                HighestEducation = dto.Role == UserRole.Mentor
                    ? dto.HighestEducation.Trim()
                    : null
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return Ok("Registered successfully.");
        }

        // ================= AGE CALCULATION =================
        private int CalculateAge(DateTime dob)
        {
            var today = DateTime.UtcNow.Date;
            var age = today.Year - dob.Year;

            if (dob.Date > today.AddYears(-age))
                age--;

            return age;
        }

        // ================= LOGIN =================
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _db.Users
                .FirstOrDefaultAsync(x => x.Username == dto.Username);

            if (user == null)
                return Unauthorized("Invalid username or password.");

            // ✅ Block check
            if (user.IsBlocked)
                return Unauthorized("Your account has been blocked. Contact admin.");

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return Unauthorized("Invalid username or password.");

            var token = JwtHelper.GenerateToken(_config, user);

            return Ok(new
            {
                token,
                role = user.Role.ToString(),
                userId = user.UserId,
                username = user.Username
            });
        }

        // ================= FORGOT PASSWORD =================
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto dto)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Email == dto.Email);
            if (user == null)
                return BadRequest("Email not registered.");

            var otp = GenerateSecureOtp();

            user.Otp = otp;
            user.OtpExpiry = DateTime.UtcNow.AddMinutes(5);

            await _db.SaveChangesAsync();

            await _emailService.SendEmailAsync(
                dto.Email,
                "Your LMS OTP Code",
                $"Your OTP is {otp}. It is valid for 5 minutes."
            );

            return Ok("OTP sent to registered email.");
        }

        // ================= VERIFY OTP =================
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp(VerifyOtpDto dto)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Email == dto.Email);
            if (user == null)
                return BadRequest("Invalid email.");

            if (user.Otp != dto.Otp)
                return BadRequest("Invalid OTP.");

            if (user.OtpExpiry == null || user.OtpExpiry < DateTime.UtcNow)
                return BadRequest("OTP expired.");

            user.Otp = null;
            user.OtpExpiry = null;

            await _db.SaveChangesAsync();

            return Ok("OTP verified successfully.");
        }

        // ================= RESET PASSWORD =================
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Email == dto.Email);
            if (user == null)
                return NotFound("User not found.");

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            await _db.SaveChangesAsync();

            return Ok("Password reset successful.");
        }

        // ================= SECURE OTP =================
        private string GenerateSecureOtp()
        {
            var bytes = RandomNumberGenerator.GetBytes(4);
            var number = BitConverter.ToUInt32(bytes, 0) % 1_000_000;
            return number.ToString("D6");
        }
    }
}
