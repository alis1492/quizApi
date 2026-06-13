using System.Data;
using System.Runtime.CompilerServices;
using FluentValidation;
using MongoDB.Bson;
using MongoDB.Driver;
using quizApi.DTOs;
using quizApi.Models;

namespace quizApi.Validators;

/// <summary>
/// Takes RootContextData["Question"] as Question
/// </summary>
public class UpdateQuestionValidator : AbstractValidator<UpdateQuestionDTO>
{
    // private readonly IMongoCollection<Quiz> _collection = collection;

    public UpdateQuestionValidator()
    {
        // NOTE: WithMessage() on RuleForEach provides the parent object, not the item.
        // ChildRules is required to access individual Answer DTO properties.
        RuleForEach(q => q.Answers).ChildRules(a =>
        {
            a.RuleFor(a => a.Id)
                .Must((dto, id, context) => {
                    var question = context.RootContextData["Question"] as Question 
                    ?? throw new Exception("Missing Question context");

                    return question.Answers.Exists(db => db.Id == new ObjectId(id));
                })
                .WithMessage(a => $"Answer with id {a.Id} not found in existing question");
        }).When(q => q.Answers != null);
    }
}