using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using quizApi.DTOs;
using quizApi.Services;

namespace quizApi.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class SubmissionController(
    ISubmissionService submissionService,
    IMapper mapper,
    ILogger<SubmissionController> logger
) : ControllerBase
{
    private readonly ISubmissionService _submissionService = submissionService;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<SubmissionController> _logger = logger;

    [HttpGet]
    public async Task<IActionResult> GetSubmissions(int? limit = null, int? skip = null)
    {
        var submissions = await _submissionService.GetSubmissions(limit, skip);
        return Ok(submissions);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSubmissionById([FromRoute] ObjectId id)
    {
        var submission = await _submissionService.GetSubmissionById(id);
        return Ok(submission);
    }

    /// <summary>
    /// For test purposes only, normally submission must be created with quizStart, 
    /// submitted with quizSubmit and graded either automatically or after some time
    /// </summary>
    /// <param name="dto">CreateSubmissionDTO</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> CreateSubmission([FromBody] CreateSubmissionDTO dto)
    {
        var submission = await _submissionService.CreateSubmission(dto);
        var submissionDto = _mapper.Map<SubmissionDTO>(submission);

        _logger.LogInformation($"Submission Created: {submissionDto.Id} (Id)");
        return Ok(submissionDto);
    }    
}