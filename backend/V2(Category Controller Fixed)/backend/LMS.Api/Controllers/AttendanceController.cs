using LMS.Api.Data;
using LMS.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

[ApiController]
[Route("api/attendance")]
[Authorize(Roles = "1")]
public class AttendanceController : ControllerBase
{
    private readonly AppDbContext _context;

    public AttendanceController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("{topicId}")]
    public IActionResult MarkAttendance(int topicId)
    {
        int studentId = int.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier));

        DateTime today = DateTime.UtcNow.Date;

        bool alreadyMarked = _context.Attendance.Any(a =>
            a.StudentId == studentId &&
            a.TopicId == topicId &&
            a.AttendanceDate == today);

        if (alreadyMarked)
            return Ok("Attendance already marked");

        var attendance = new Attendance
        {
            StudentId = studentId,
            TopicId = topicId,
            AttendanceDate = today,
            Status = "Present"
        };

        _context.Attendance.Add(attendance);
        _context.SaveChanges();

        return Ok("Attendance marked");
    }
}
