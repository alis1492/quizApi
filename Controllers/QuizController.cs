using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
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

    [HttpGet(Name = "GetAll")]
    public async Task<IActionResult> GetQuizzes([FromQuery] int? limit,[FromQuery]  int? skip)
    {
        List<QuizDTO> quizzes = await _quizService.GetQuizzes(limit, skip);
        return Ok(quizzes);
    }

    [HttpGet("{id}", Name = "GetById")]
    public async Task<IActionResult> GetQuizById([FromRoute] string id)
    {
        QuizDTO quiz = await _quizService.GetQuizById(new ObjectId(id));
        return Ok(quiz);
    }

    [HttpPost]
    public async Task<IActionResult> CreateQuiz([FromBody]CreateQuizDTO dto)
    {
        Quiz quiz = await _quizService.CreateQuiz(dto);

        _logger.LogInformation($"Quiz Created: {quiz.Id} (Id)");
        return Ok(new { QuizId = quiz.Id.ToString() });
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateQuiz([FromRoute]string id, [FromBody]UpdateQuizDTO dto)
    {
        Quiz quiz = await _quizService.UpdateQuiz(id, dto);

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

    // Question managment
    [HttpPost("{id}/question")]
    public async Task<IActionResult> AddQuestion([FromRoute] string id, [FromBody] CreateQuestionDTO dto)
    {
        Question question = await _quizService.AddQuestion(id, dto);
        _logger.LogInformation($"Question Added: {id} (Quiz Id)\n\t{question.Id} (Question Id)");

        return Created();
    }
}