using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using System.Text.Json;

namespace DietManagementSystem.API.Middlewares;
public class ValidationExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is ValidationException validationException)
        {
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            httpContext.Response.ContentType = "application/json";

            var errors = validationException.Errors.Select(e => new
            {
                e.PropertyName,
                e.ErrorMessage
            });

            var response = JsonSerializer.Serialize(new { Errors = errors });

            await httpContext.Response.WriteAsync(response, cancellationToken);

            return true;
        }

        return false;
    }
}

