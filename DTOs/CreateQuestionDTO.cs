using System.ComponentModel.DataAnnotations;

namespace quizApi.DTOs;

public class CreateQuestionDTO
{
    // hardcoded range based on Models.QType
    // not scalable but works for mvp
    [Range(0, 1)]
    public required int Type { get; set; }
    public required string Text { get; set; }
    public required int Points { get; set; }
    [Length(2, 10)]
    public required List<CreateAnswerDTO> Answers { get; set; }
}