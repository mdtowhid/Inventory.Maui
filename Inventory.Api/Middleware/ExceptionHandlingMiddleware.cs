using FluentValidation;
using Inventory.Application.Common.Exceptions;
using System.Text.Json;

namespace Inventory.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "An unhandled exception occurred");

        var response = context.Response;
        response.ContentType = "application/json";

        var (statusCode, message) = exception switch
        {
            ValidationException validationException =>
                (StatusCodes.Status400BadRequest, GetValidationErrors(validationException)),
            NotFoundException notFoundException =>
                (StatusCodes.Status404NotFound, notFoundException.Message),
            UnauthorizedAccessException _ =>
                (StatusCodes.Status401Unauthorized, "You are not authorized to access this resource"),
            InvalidOperationException invalidOperation =>
                (StatusCodes.Status400BadRequest, invalidOperation.Message),
            _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred")
        };

        response.StatusCode = statusCode;

        var errorResponse = new
        {
            statusCode,
            message,
            timestamp = DateTime.UtcNow
        };

        await response.WriteAsync(JsonSerializer.Serialize(errorResponse));
    }

    private static string GetValidationErrors(ValidationException exception)
    {
        var errors = exception.Errors.Select(e => e.ErrorMessage);
        return string.Join("; ", errors);
    }
}