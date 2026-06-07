using FluentValidation;
using quizApi.DTOs;
using quizApi.Models;

namespace quizApi.Validators;

public class CreateQuestionValidator : AbstractValidator<CreateQuestionDTO>
{
    public CreateQuestionValidator()
    {
        RuleFor(q => q.Answers)
            .Must(ContainOneCorrectAnswer)
                .When(q => q.Type == (int)QType.singleChoice)
                .WithMessage("There must be one correct answer");
        
        RuleFor(q => q.Answers)
            .Must(ContainMultipleCorrectAnswers)
                .When(q => q.Type == (int)QType.multipleChoice)
                .WithMessage("There must be more than one correct answer");
    }

    public bool ContainOneCorrectAnswer(List<CreateAnswerDTO> dto)
    {
        return dto.Count(a => a.IsCorrect == true) == 1;
    }

    public bool ContainMultipleCorrectAnswers(List<CreateAnswerDTO> dto)
    {
        return dto.Count(a => a.IsCorrect == true) > 1;
    }
}