using AutoMapper;
using quizApi.DTOs;
using quizApi.Extensions;
using quizApi.Models;
using quizApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// *** Custom Services ***
builder.Services
    .AddMongoClient()
    .AddDatabase()
    .AddCollection<Quiz>("quizes");
builder.Services.AddScoped<IQuizService, QuizService>();
builder.Services.AddAutoMapper(cfg => { }, typeof(Program).Assembly);
// *** Custom Services ***

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

// AutoMapper profile
class AutoMapperProfile : Profile
{   
    public AutoMapperProfile()
    {
        CreateMap<Quiz, QuizDTO>();
        CreateMap<Question, QuestionDTO>();
        CreateMap<Answer, AnswerDTO>();
    }
}