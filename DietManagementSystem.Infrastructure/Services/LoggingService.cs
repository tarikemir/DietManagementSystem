using DietManagementSystem.Application.Services;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Context;

namespace DietManagementSystem.Infrastructure.Services;

public class LoggingService : ILoggingService
{
    private readonly ILogger<LoggingService> _logger;
    private readonly IDiagnosticContext _diagnosticContext;

    public LoggingService(ILogger<LoggingService> logger, IDiagnosticContext diagnosticContext)
    {
        _logger = logger;
        _diagnosticContext = diagnosticContext;
    }

    public void LogInformation(string message, params object[] args)
    {
        _logger.LogInformation(message, args);
    }

    public void LogWarning(string message, params object[] args)
    {
        _logger.LogWarning(message, args);
    }

    public void LogError(string message, params object[] args)
    {
        _logger.LogError(message, args);
    }

    public void LogError(Exception exception, string message, params object[] args)
    {
        _logger.LogError(exception, message, args);
    }

    public void LogDebug(string message, params object[] args)
    {
        _logger.LogDebug(message, args);
    }

    public void LogTrace(string message, params object[] args)
    {
        _logger.LogTrace(message, args);
    }

    public void LogCritical(string message, params object[] args)
    {
        _logger.LogCritical(message, args);
    }

    public void LogCritical(Exception exception, string message, params object[] args)
    {
        _logger.LogCritical(exception, message, args);
    }

    public void LogWithContext(string operation, string entity, Guid? entityId = null, string userId = null)
    {
        using (LogContext.PushProperty("Operation", operation))
        using (LogContext.PushProperty("Entity", entity))
        {
            if (entityId.HasValue)
                LogContext.PushProperty("EntityId", entityId.Value);
            if (!string.IsNullOrEmpty(userId))
                LogContext.PushProperty("UserId", userId);
        }
    }
} 