using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace quizApi.Models;

public enum UserType
{
    Student = 0,
    Teacher = 1,
    Admin = 2
}

// zeroed by default, populated after grading
public class Score
{
    public double TotalPoints {get;set;} = 0;
    public double MaxPoints {get; set;} = 0;
    public double Percentage {get; set;} = 0;
}

public enum Status
{
    InProgress = 0,
    Submitted = 1,
    Graded = 2,
    Expired = 3 // after quiz update for example
}

[BsonIgnoreExtraElements]
public class Submission
{
    public ObjectId Id {get; set;} = ObjectId.GenerateNewId();
    public required ObjectId QuizId {get; set;}
    [BsonRepresentation(BsonType.Int32)]
    public required UserType UserType {get; set;}
    public required ObjectId UserId {get; set;}
    // Utc in name so that future me doesnt accidentally use DateTime.Now accidentally
    public DateTime StartedAtUtc {get;set;} = DateTime.UtcNow;
    public DateTime? SubmittedAtUtc {get; set;}
    public Score Score {get; set;} = new Score();
    public List<UserAnswer> UserAnswers {get; set;} = [];
    [BsonRepresentation(BsonType.Int32)]
    public Status Status {get; set;} = Status.InProgress;
}