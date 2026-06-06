using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using quizApi.DTOs;
using quizApi.Models;
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
        foreach(Quiz q in quizzes)
        {
            string json = JsonSerializer.Serialize(q, new JsonSerializerOptions{WriteIndented = true});
            _logger.LogInformation(json);
        }
        return Ok(quizzes);
    }
}