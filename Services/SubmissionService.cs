using quizApi.Models;
using quizApi.DTOs;
using MongoDB.Driver;
using AutoMapper;
using quizApi.Exceptions;
using MongoDB.Bson;

namespace quizApi.Services;

public class SubmissionService(
    IMongoCollection<Submission> submissionCollection,
    IMongoCollection<Quiz> quizCollection,
    IMapper mapper
) : ISubmissionService
{
    private readonly IMongoCollection<Submission> _submissionCollection = submissionCollection;
    private readonly IMongoCollection<Quiz> _quizCollection = quizCollection;
    private readonly IMapper _mapper = mapper;

    // Get submissions
    /// <summary>
    /// Get all submissions as dtos, can return empty list
    /// </summary>
    /// <param name="limit"></param>
    /// <param name="skip"></param>
    /// <returns></returns>
    public async Task<List<SubmissionDTO>> GetSubmissions(int? limit = null, int? skip = null)
    {
        var submissions = await GetFullSubmissions(limit, skip);

        // map them to DTO
        List<SubmissionDTO> submissionDTOs = [];
        foreach (var s in submissions)
        {
            var sDTO = _mapper.Map<SubmissionDTO>(s);
            submissionDTOs.Add(sDTO);
        }

        // return list of dtos
        return submissionDTOs;
    }
    /// <summary>
    /// Get all full submissions, can return empty list
    /// </summary>
    /// <param name="limit"></param>
    /// <param name="skip"></param>
    /// <returns></returns>
    public async Task<List<Submission>> GetFullSubmissions(int? limit = null, int? skip = null)
    {
        var submissions = await _submissionCollection
            .Find(_ => true)
            .Limit(limit)
            .Skip(skip)
            .ToListAsync();
        return submissions;
    }

    // Get individual submission
    /// <summary>
    /// Get submissionDTO or throw
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="DocumentNotFoundException"></exception>
    public async Task<SubmissionDTO> GetSubmissionById(ObjectId id)
    {
        var submission = await GetFullSubmissionById(id);
        return _mapper.Map<SubmissionDTO>(submission);
    }
    /// <summary>
    /// Get full submission or throw
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="DocumentNotFoundException"></exception>
    public async Task<Submission> GetFullSubmissionById(ObjectId id)
    {
        var submission = await _submissionCollection
            .Find(s => s.Id == id)
            .FirstOrDefaultAsync()
            ?? throw new DocumentNotFoundException("Quiz", id.ToString());
        return submission;
    }

    /// <summary>
    /// Create a new submission, which status = InProgress 
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    /// <exception cref="DocumentNotFoundException">when QuizId is invalid</exception>
    public async Task<Submission> CreateSubmission(CreateSubmissionDTO dto)
    {
        var submission = _mapper.Map<Submission>(dto);
        // checks go after mapping because string to ObjectId validates string automatically
        // Quiz check
        var quiz = await _quizCollection
            .Find<Quiz>(q => q.Id == submission.QuizId)
            .FirstOrDefaultAsync()
            ?? throw new DocumentNotFoundException("Quiz", dto.QuizId);

        // TODO: User check once auth gets implemented

        await _submissionCollection.InsertOneAsync(submission);
        return submission;
    }


    // public Task<Quiz> UpdateQuiz(ObjectId Id, UpdateQuizDTO dto);
    // public Task DeleteQuiz(ObjectId Id);
}