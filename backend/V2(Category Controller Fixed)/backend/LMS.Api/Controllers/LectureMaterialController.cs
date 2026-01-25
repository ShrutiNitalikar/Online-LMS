using LMS.Api.Data;
using LMS.Api.Models;
using LMS.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Api.Controllers;

[ApiController]
[Route("api/materials")]
[Authorize(Roles = "2")] // Teacher only
public class LectureMaterialController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly FileStorageService _fileService;

    public LectureMaterialController(
        AppDbContext context,
        FileStorageService fileService)
    {
        _context = context;
        _fileService = fileService;
    }

    [HttpPost("{topicId}")]
    public async Task<IActionResult> UploadMaterial(
        int topicId,
        [FromForm] IFormFile file,
        [FromForm] string type)
    {
        if (file == null || file.Length == 0)
            return BadRequest("File is required");

        var url = await _fileService.UploadAsync(file, "materials");

        var material = new LectureMaterial
        {
            TopicId = topicId,
            Type = type,
            Url = url,
            UploadedOn = DateTime.UtcNow
        };

        _context.LectureMaterials.Add(material);
        _context.SaveChanges();

        return Ok(material);
    }
}
