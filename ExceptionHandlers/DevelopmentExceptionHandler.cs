using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Diagnostics;
using MongoDB.Driver;

namespace quizApi.ExceptionHandlers;

// Logs everything and returns informative error messages to help frontend team
public class DevelopmentExceptionHandler(
    ILogger<DevelopmentExceptionHandler> logger
) : IExceptionHandler
{
    private readonly ILogger<DevelopmentExceptionHandler> _logger = logger;

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        httpContext.Response.StatusCode = 400;
        httpContext.Response.ContentType = "application/problem+json";
        if(exception is FluentValidation.ValidationException || exception is ValidationException)
        {
            var payload = new
            {
                Title = "Validation Error",
                Status = 400,
                exception.Message,
                exception.StackTrace,
                Timestamp = DateTime.UtcNow.ToString("o")
            };
            await httpContext.Response.WriteAsJsonAsync(payload, cancellationToken);
        } else if(exception is MongoException)
        {
            var payload = new
            {
                Title = "Database Error",
                Status = 500,
                exception.Message,
                exception.StackTrace,
                Timestamp = DateTime.UtcNow.ToString("o")
            };
            httpContext.Response.StatusCode = 500;
            await httpContext.Response.WriteAsJsonAsync(payload, cancellationToken);
        }

        // * true means handled
        // * false means unhandled - rethrow
        return true;
    }
}