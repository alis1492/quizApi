using System.ComponentModel.DataAnnotations;
using quizApi.Models;

namespace quizApi.DTOs;

public class CreateSubmissionDTO
{
    public required string QuizId {get; set;}
    [Range(0, (int)Models.UserType.Admin)]
    public required int UserType {get; set;}
    public required string UserId {get; set;}
}