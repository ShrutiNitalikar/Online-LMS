using LMS.Api.Data;
using LMS.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Api.Controllers;

[ApiController]
[Route("api/assignments")]
[Authorize(Roles = "2")]
public class AssignmentController : ControllerBase
{
    private readonly AppDbContext _context;

    public AssignmentController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public IActionResult CreateAssignment(Assignment assignment)
    {
        _context.Assignments.Add(assignment);
        _context.SaveChanges();

        return Ok(assignment);
    }
}
