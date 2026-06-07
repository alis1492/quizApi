using System.Threading.Tasks;
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

    public async Task<Quiz> CreateQuiz(CreateQuizDTO dto)
    {
        var quiz = new Quiz
        {
            Title = dto.Title,
            Description = dto.Description,
            TeacherId = new ObjectId(dto.TeacherId),
            // dto validation allows unsafe cast
            Subject = (Models.Subject)dto.Subject,
            Settings = dto.Settings
        };

        // if this doesnt throw everything is fine
        await _collection.InsertOneAsync(quiz);

        return quiz;
    }

    public async Task<Quiz> UpdateQuiz(UpdateQuizDTO dto, string id)
    {
        // helper
        var updateBuilder = Builders<Quiz>.Update;
        // update definitions to be combined
        List<UpdateDefinition<Quiz>> updateDefinitions = [];
        
        // foreach not null field add an UpdateDefinition to the list
        if(dto.Title != null)
            updateDefinitions.Add(updateBuilder.Set(q => q.Title, dto.Title));
        if(dto.Description != null)
            updateDefinitions.Add(updateBuilder.Set(q => q.Description, dto.Description));
        if(dto.Subject != null)
            updateDefinitions.Add(updateBuilder.Set(q => q.Subject, (Subject)dto.Subject));
        if(dto.Settings != null)
            updateDefinitions.Add(updateBuilder.Set(q => q.Settings, dto.Settings));

        // combine all UpdateDefinitions into one
        var update = updateBuilder.Combine(updateDefinitions);

        // if doesnt throw everything is fine
        var quiz = await _collection.FindOneAndUpdateAsync(
            q => q.Id == new ObjectId(id),
            update,
            // ReturnDocument allows the same db call to return updated document
            new FindOneAndUpdateOptions<Quiz>{ReturnDocument = ReturnDocument.After}
        );

        return quiz;
    }

    public async Task DeleteQuiz(string id)
    {
        // fine if doesnt throw
        await _collection.FindOneAndDeleteAsync(q => q.Id == new ObjectId(id));
    }
}