namespace Application.Services.LogService;

public interface ILoggerService
{
    void LogInfo(string message);
    void LogWarning(string message);
    void LogError(string message , Exception ex);
}