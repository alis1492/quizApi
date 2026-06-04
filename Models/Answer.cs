using MongoDB.Bson;

namespace quizApi.Models;

public class Answer
{
    public ObjectId Id { get; set; }
    public required string Text { get; set; }
    public required bool IsCorrect { get; set; }
}