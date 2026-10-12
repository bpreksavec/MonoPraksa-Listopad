using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using WebApplication3.Exceptions;

namespace WebApplication3.Middleware;
public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly IHostEnvironment _env;
    private readonly IProblemDetailsService _problemDetailsService;

    public GlobalExceptionHandler(
        ILogger<GlobalExceptionHandler> logger,
        IHostEnvironment env,
        IProblemDetailsService problemDetailsService)
    {
        _logger = logger;
        _env = env;
        _problemDetailsService = problemDetailsService;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var problem = CreateProblemDetails(exception);

        if (problem.Status >= StatusCodes.Status500InternalServerError)
        {
            _logger.LogError(exception, "Unhandled exception for {Method} {Path}",
                httpContext.Request.Method, httpContext.Request.Path);
        }
        else
        {
            _logger.LogInformation("Request failed with {Status}: {Message}",
                problem.Status, exception.Message);
        }

        httpContext.Response.StatusCode = problem.Status!.Value;

        return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = problem,
            Exception = exception
        });
    }

    private ProblemDetails CreateProblemDetails(Exception exception)
    {
        switch (exception)
        {
            case ValidationFailedException validation:
                return new ValidationProblemDetails(validation.Errors)
                {
                    Status = validation.StatusCode,
                    Title = validation.Message
                };

            case AppException app:
                return new ProblemDetails
                {
                    Status = app.StatusCode,
                    Title = app.Message
                };

            case DbUpdateException { InnerException: PostgresException pg }
                when pg.SqlState == PostgresErrorCodes.UniqueViolation:
                return new ProblemDetails
                {
                    Status = StatusCodes.Status409Conflict,
                    Title = "The record conflicts with existing data."
                };

            case DbUpdateException { InnerException: PostgresException pg }
                when pg.SqlState == PostgresErrorCodes.ForeignKeyViolation:
                return new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "A referenced record does not exist."
                };

            case BadHttpRequestException badRequest:
                return new ProblemDetails
                {
                    Status = badRequest.StatusCode,
                    Title = "The request was malformed."
                };

            case OperationCanceledException:
                return new ProblemDetails
                {
                    Status = 499,
                    Title = "The request was cancelled."
                };

            default:
                return new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "An unexpected error occurred.",
                    Detail = _env.IsDevelopment() ? exception.ToString() : null
                };
        }
    }
}