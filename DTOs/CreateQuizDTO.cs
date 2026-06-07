using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using quizApi.Models;

namespace quizApi.DTOs;

public class CreateQuizDTO
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string TeacherId { get; set; }
    // Simple validation, 0 is always the first type in enum, Subject.Other is always the last
    [Range(
        0,
        (int)Models.Subject.Other
    )]
    public int Subject { get; set; } = (int)Models.Subject.Other;
    public QuizSettings Settings { get; set; } = new QuizSettings();
}