using System.ComponentModel.DataAnnotations;

namespace quizApi.DTOs;

public class UpdateQuestionDTO
{
    [Range(0, 1)]
    public int? Type { get; set; }
    public string? Text { get; set; }
    [Range(0, 100)]
    public int? Points { get; set; }
    public List<UpdateAnswerDTO>? Answers { get; set; }
}