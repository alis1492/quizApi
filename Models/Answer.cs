using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace quizApi.Models;

[BsonIgnoreExtraElements]
public class Answer
{
    public ObjectId Id { get; set; }
    public required string Text { get; set; }
    public required bool IsCorrect { get; set; }
}