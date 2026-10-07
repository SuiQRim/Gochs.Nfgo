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

            var response = new
            {
                status = statusCode,
                title = statusCode switch
                {
                    StatusCodes.Status404NotFound => "Resource not found.",
                    StatusCodes.Status409Conflict => "Business rule violation.",
                    _ => "An unexpected error occurred."
                },
                detail = statusCode == StatusCodes.Status500InternalServerError && !environment.IsDevelopment()
                    ? "Internal server error"
                    : exception.Message
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
