//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Online_LMS.Data;
//using Online_LMS.Models;

//namespace Online_LMS.Controllers
//{
//    [ApiController]
//    [Route("api/admin/users")]
//    [Authorize(Roles = "Admin")]
//    public class AdminUsersController : ControllerBase
//    {
//        private readonly AppDbContext _db;

//        public AdminUsersController(AppDbContext db)
//        {
//            _db = db;
//        }

//        [HttpGet("students")]
//        public async Task<IActionResult> GetStudents()
//        {
//            var list = await _db.Users
//                .Where(x => x.Role == UserRole.Student)
//                .OrderByDescending(x => x.UserId)
//                .ToListAsync();

//            return Ok(list);
//        }

//        [HttpGet("mentors")]
//        public async Task<IActionResult> GetMentors()
//        {
//            var list = await _db.Users
//                .Where(x => x.Role == UserRole.Mentor)
//                .OrderByDescending(x => x.UserId)
//                .ToListAsync();

//            return Ok(list);
//        }

//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteUser(int id)
//        {
//            var user = await _db.Users.FindAsync(id);
//            if (user == null) return NotFound("User not found.");

//            _db.Users.Remove(user);
//            await _db.SaveChangesAsync();

//            return Ok("User deleted.");
//        }
//    }

//}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_LMS.Data;
using Online_LMS.Models;

namespace Online_LMS.Controllers
{
    [ApiController]
    [Route("api/admin/users")]
    [Authorize(Roles = "Admin")]
    public class AdminUsersController : ControllerBase
    {
        private readonly AppDbContext _db;

        public AdminUsersController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet("students")]
        public async Task<IActionResult> GetStudents()
        {
            var list = await _db.Users
                .Where(x => x.Role == UserRole.Student)
                .OrderByDescending(x => x.UserId)
                .Select(u => new
                {
                    u.UserId,
                    u.FirstName,
                    u.LastName,
                    u.Email,
                    u.Username,
                    u.MobileNumber,
                    u.IsBlocked,
                    u.CreatedAt
                })
                .ToListAsync();

            return Ok(list);
        }

        [HttpGet("mentors")]
        public async Task<IActionResult> GetMentors()
        {
            var list = await _db.Users
                .Where(x => x.Role == UserRole.Mentor)
                .OrderByDescending(x => x.UserId)
                .Select(u => new
                {
                    u.UserId,
                    u.FirstName,
                    u.LastName,
                    u.Email,
                    u.Username,
                    u.MobileNumber,
                    u.IsBlocked,
                    u.CreatedAt
                })
                .ToListAsync();

            return Ok(list);
        }

        // ✅ Block user (no DB delete)
        [HttpPut("{id}/block")]
        public async Task<IActionResult> BlockUser(int id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null) return NotFound(new { message = "User not found." });

            if (user.Role == UserRole.Admin)
                return BadRequest(new { message = "Admin cannot be blocked." });

            user.IsBlocked = true;
            await _db.SaveChangesAsync();

            return Ok(new { message = "User blocked successfully." });
        }

        // ✅ Unblock user
        [HttpPut("{id}/unblock")]
        public async Task<IActionResult> UnblockUser(int id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null) return NotFound(new { message = "User not found." });

            user.IsBlocked = false;
            await _db.SaveChangesAsync();

            return Ok(new { message = "User unblocked successfully." });
        }
    }
}

