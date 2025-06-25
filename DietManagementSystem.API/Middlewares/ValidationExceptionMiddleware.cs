using System.Text.Json;

namespace DietManagementSystem.API.Middlewares;
public class ValidationExceptionMiddleware
{
    private readonly RequestDelegate _next;
    public ValidationExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (FluentValidation.ValidationException ex)
        {
            context.Response.StatusCode = 400;
            context.Response.ContentType = "application/json";
            var errors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
            var result = JsonSerializer.Serialize(new { Errors = errors });
            await context.Response.WriteAsync(result);
        }
    }
}
