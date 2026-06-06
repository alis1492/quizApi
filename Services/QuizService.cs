using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;
using quizApi.DTOs;
using quizApi.Models;

namespace quizApi.Services;

public class QuizService(IMongoCollection<Quiz> collection, IMapper mapper) : IQuizService
{
    private readonly IMongoCollection<Quiz> _collection = collection;
    private readonly IMapper _mapper = mapper;

    // Get all quizes asyncronously
    public async Task<List<QuizDTO>> GetQuizzes(int? limit = null, int? skip = null)
    {
        // get all full quizes
        var quizzes = await GetFullQuizzes(limit, skip);

        // map them to DTO
        List<QuizDTO> quizDTOs = [];
        foreach (var quiz in quizzes)
        {
            var quizDTO = _mapper.Map<QuizDTO>(quiz);
            quizDTOs.Add(quizDTO);
        }

        // return list of dtos
        return quizDTOs;
    }
    public async Task<List<Quiz>> GetFullQuizzes(int? limit = null, int? skip = null)
    {
        var quizzes = await _collection
            .Find(_ => true)
            .Limit(limit)
            .Skip(skip)
            .ToListAsync();
        return quizzes;
    }

    // Get a single quiz by id
    public async Task<QuizDTO> GetQuizById(ObjectId id)
    {
        var quiz = await GetFullQuizById(id);
        return _mapper.Map<QuizDTO>(quiz);
    }
    public async Task<Quiz> GetFullQuizById(ObjectId id)
    {
        return await _collection.Find<Quiz>(q => q.Id == id).FirstOrDefaultAsync();
    }
}