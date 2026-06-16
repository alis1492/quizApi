using quizApi.Models;
using quizApi.DTOs;
using MongoDB.Driver;
using AutoMapper;
using quizApi.Exceptions;

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

    // public Task<List<QuizDTO>> GetQuizzes(int? limit = null, int? skip = null);
    // public Task<List<Quiz>> GetFullQuizzes(int? limit = null, int? skip = null);

    // // Get a single quiz by id
    // public Task<QuizDTO> GetQuizById(ObjectId id);
    // public Task<Quiz> GetFullQuizById(ObjectId id);
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