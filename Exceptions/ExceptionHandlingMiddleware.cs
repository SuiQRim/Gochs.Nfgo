using Gochs.Nfgo.DTOs.Common;

namespace Gochs.Nfgo.Exceptions;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger<ExceptionHandlingMiddleware> logger;
    private readonly IHostEnvironment environment;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger,
        IHostEnvironment environment)
    {
        this.next = next;
        this.logger = logger;
        this.environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            var statusCode = exception switch
            {
                ValidationException => StatusCodes.Status400BadRequest,
                NotFoundException => StatusCodes.Status404NotFound,
                BusinessRuleException => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError
            };

            if (statusCode == StatusCodes.Status500InternalServerError)
                logger.LogError(exception, "Unhandled exception");
            else
                logger.LogWarning(exception, "Request failed with status code {StatusCode}", statusCode);

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var response = new ErrorResponseDto
            {
                Status = statusCode,
                Title = statusCode switch
                {
                    StatusCodes.Status400BadRequest => "Ошибка валидации.",
                    StatusCodes.Status404NotFound => "Ресурс не найден.",
                    StatusCodes.Status409Conflict => "Нарушено бизнес-правило.",
                    _ => "Произошла внутренняя ошибка."
                },
                Detail = statusCode == StatusCodes.Status500InternalServerError && !environment.IsDevelopment()
                    ? "Внутренняя ошибка сервера"
                    : exception.Message
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
