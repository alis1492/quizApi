using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace quizApi.Models;

public enum Subject
{
    Math = 0,
    Science = 1,
    History = 2,
    Programming = 3,
    Language = 4,
    Other = 5
}
public class QuizSettings
{
    public bool ShuffleQuestions { get; set; } = false;
    public bool ShuffleAnswers { get; set; } = true;
    public int? DefaultQuestionTimeLimit { get; set; }
    public int? TimeLimit { get; set; }
}
[BsonIgnoreExtraElements]
public class Quiz
{
    public ObjectId Id;
    public required string Title { get; set; }
    public required string Description { get; set; }
    public ObjectId TeacherId { get; set; }
    [BsonRepresentation(BsonType.Int32)]
    public Subject Subject { get; set; } = Subject.Other;
    public required QuizSettings Settings;
    // Questions are nullable so its possible to create a test
    // and then create questions one by one
    public List<Question>? Questions;
}