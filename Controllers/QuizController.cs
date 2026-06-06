using Microsoft.AspNetCore.Mvc;
using quizApi.DTOs;
using quizApi.Services;

namespace quizApi.Controllers;

[ApiController]
[Route("[controller]")]
public class QuizController(IQuizService quizService, ILogger<QuizController> logger) : ControllerBase
{
    private readonly IQuizService _quizService = quizService;
    private readonly ILogger<QuizController> _logger = logger;

    [HttpGet(Name = "GetQuizzes")]
    public async Task<IActionResult> GetQuizzes()
    {
        var quizzes = await _quizService.GetFullQuizzes();
        return Ok(quizzes);
    }
}