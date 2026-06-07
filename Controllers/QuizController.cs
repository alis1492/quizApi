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
        List<QuizDTO> quizzes = await _quizService.GetQuizzes();
        return Ok(quizzes);
    }

    [HttpPost]
    public async Task<IActionResult> CreateQuiz([FromBody]CreateQuizDTO dto)
    {
        Quiz quiz = await _quizService.CreateQuiz(dto);

        _logger.LogInformation($"Quiz Created: {quiz.Id} (Id)");
        return Ok(new { QuizId = quiz.Id.ToString() });
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateQuiz([FromBody]UpdateQuizDTO dto, [FromRoute]string id)
    {
        Quiz quiz = await _quizService.UpdateQuiz(dto, id);

        _logger.LogInformation($"Quiz Updated: {id} (Id)");
        return Created();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteQuiz([FromRoute]string id)
    {
        await _quizService.DeleteQuiz(id);
        _logger.LogInformation($"Quiz Deleted: {id} (Id)");
        
        return Ok();
    }
}