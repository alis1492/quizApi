using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace quizApi.Models;

public enum QType
{
    singleChoice = 0,
    multipleChoice = 1
}

public class Question
{
    public ObjectId Id { get; set; }
    [BsonRepresentation(BsonType.Int32)]
    public required QType Type { get; set; }
    public required string Text { get; set; }
    public required int Points { get; set; }
    public required List<Answer> Answers { get; set; }
}