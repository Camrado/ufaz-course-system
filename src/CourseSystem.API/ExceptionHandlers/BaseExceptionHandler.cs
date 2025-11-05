using CourseSystem.Exceptions.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

namespace CourseSystem.API.ExceptionHandlers;

public class BaseExceptionHandler : IExceptionHandler
{
    private readonly IProblemDetailsService _problemDetailsService;
    private readonly ILogger<BaseExceptionHandler> _logger;

    public BaseExceptionHandler(IProblemDetailsService problemDetailsService, ILogger<BaseExceptionHandler> logger)
    {
        _problemDetailsService = problemDetailsService;
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not BaseException baseException)
        {
            return false;
        }

        httpContext.Response.StatusCode = baseException switch
        {
            NotFoundException => StatusCodes.Status404NotFound,
            ConflictException => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status400BadRequest,
        };

        _logger.LogError(exception,
            "A base exception occured. \n" +
            "Status Code: {StatusCode} \n" +
            "Request Method: {RequestMethod}\n" +
            "Request Path: {RequestPath}\n" +
            "Request Id: {RequestId}\n" +
            "Trace Id: {TraceId}\n" +
            "Error Message: {ErrorMessage}",
            httpContext.Response.StatusCode,
            httpContext.Request.Method,
            httpContext.Request.Path,
            httpContext.TraceIdentifier,
            httpContext.Features.Get<IHttpActivityFeature>()?.Activity.Id,
            exception.Message);

        await _problemDetailsService.WriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = new ProblemDetails
            {
                Type = baseException.GetType().Name,
                Title = "An error occurred.",
                Detail = baseException.Parameters.Length > 0
                    ? string.Format(baseException.Message, baseException.Parameters)
                    : baseException.Message,
            }
        });

        return true;
    }
}
