using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using quizApi.DTOs;
using quizApi.Models;
using quizApi.Services;

namespace quizApi.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class SubmissionController(
    ISubmissionService submissionService,
    IMapper _mapper
) : ControllerBase
{
    private readonly ISubmissionService _submissionService = submissionService;

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
        return Ok(submissionDto);
    }    
}