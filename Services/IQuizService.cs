using MongoDB.Bson;
using quizApi.DTOs;
using quizApi.Models;

namespace quizApi.Services;

public interface IQuizService
{
    // Get all quizes asyncronously
    public Task<List<QuizDTO>> GetQuizzes(int? limit = null, int? skip = null);
    public Task<List<Quiz>> GetFullQuizzes(int? limit = null, int? skip = null);

    // Get a single quiz by id
    public Task<QuizDTO> GetQuizById(ObjectId id);
    public Task<Quiz> GetFullQuizById(ObjectId id);
    public Task<Quiz> CreateQuiz(CreateQuizDTO dto);
    public Task<Quiz> UpdateQuiz(ObjectId Id, UpdateQuizDTO dto);
    public Task DeleteQuiz(ObjectId Id);
    public Task<Question> AddQuestion(ObjectId quizId, CreateQuestionDTO dto);
    public Task<Question> UpdateQuestion(ObjectId quizId, ObjectId questionId, UpdateQuestionDTO dto);
    public Task DeleteQuestion(ObjectId quizId, ObjectId questionId);
}