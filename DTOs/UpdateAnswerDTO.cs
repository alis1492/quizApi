using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace quizApi.DTOs;

/// <summary>
/// FULL answer document, Question.Answers is fully replaced on update after validation
/// </summary>
public class UpdateAnswerDTO
{
    [Length(24, 24)]
    public required ObjectId Id { get; set; }
    public required string Text { get; set; }
    public required bool IsCorrect { get; set; }
}