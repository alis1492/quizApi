using quizApi.Models;

namespace quizApi.DTOs;

public class QuestionDTO
{
    public required string Id {get; set;}
    public required QType Type { get; set; }
    public required string Text { get; set; }
    public required int Points { get; set; }
    public required List<AnswerDTO> Answers { get; set; }
}