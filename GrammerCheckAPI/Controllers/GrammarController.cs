//using Microsoft.AspNetCore.Mvc;

//[ApiController]
//[Route("api/grammar")]
//public class GrammarController : ControllerBase
//{
//    private readonly GrammarCheckService _grammarService;

//    public GrammarController(GrammarCheckService grammarService)
//    {
//        _grammarService = grammarService;
//    }

//    [HttpPost("check")]
//    public async Task<IActionResult> CheckGrammar([FromBody] GrammarRequest request)
//    {
//        if (string.IsNullOrWhiteSpace(request.Text))
//            return BadRequest("Text cannot be empty.");

//        var result = await _grammarService.CheckGrammarAsync(request.Text);
//        return Ok(result);
//    }
//}

//public class GrammarRequest
//{
//    public string Text { get; set; } = string.Empty;
//}
using Microsoft.AspNetCore.Mvc;
using LmsGrammarAI.Services;

namespace LmsGrammarAI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GrammarController : ControllerBase
    {
        private readonly GeminiGrammarService _grammarService;

        public GrammarController(GeminiGrammarService grammarService)
        {
            _grammarService = grammarService;
        }

        [HttpPost("correct")]
        public async Task<IActionResult> CorrectGrammar([FromBody] GrammarRequest request)
        {
            if (string.IsNullOrEmpty(request.Text))
                return BadRequest("Text is required");

            var corrected = await _grammarService.CorrectGrammar(request.Text);

            return Ok(new { corrected });
        }
    }

    public class GrammarRequest
    {
        public string Text { get; set; }
    }
}
