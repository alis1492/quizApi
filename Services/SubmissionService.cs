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
    public async Task<SubmissionDTO> GetSubmissionById(ObjectId id)
    {
        var submission = await GetFullSubmissionById(id);
        return _mapper.Map<SubmissionDTO>(submission);
    }
    
    public async Task<Submission> GetFullSubmissionById(ObjectId id)
    {
        var submission = await _submissionCollection
            .Find(s => s.Id == id)
            .FirstOrDefaultAsync()
            ?? throw new DocumentNotFoundException("Quiz", id.ToString());
        return submission;
    }

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

    public async Task DeleteSubmission(ObjectId id)
    {
        var result = await _submissionCollection.DeleteOneAsync(s => s.Id == id);
        if(result.DeletedCount <= 0)
            throw new MongoException($"Failed to delete submission with id {id}");
    }

    public async Task<Submission> SubmitAnswers(ObjectId id, List<SubmitUserAnswerDTO> answerDTOs)
    {
        List<UserAnswer> answers = [];
        foreach(var answerDTO in answerDTOs)
        {
            var answer = _mapper.Map<UserAnswer>(answerDTO);
            answers.Add(answer);
        }
        var update = Builders<Submission>.Update
            .PushEach(s => s.UserAnswers, answers)
            .Set(s => s.Status, Status.Submitted);
        var submission = await _submissionCollection.FindOneAndUpdateAsync(s => s.Id == id, update,
            new FindOneAndUpdateOptions<Submission, Submission>{ReturnDocument = ReturnDocument.After})
            ?? throw new MongoException($"Failed to submit answers to quiz with id {id}");
        
        return submission;
    }
}