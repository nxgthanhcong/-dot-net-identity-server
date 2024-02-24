using Core.Models.LoggingModels;

namespace Core.Logging.Interfaces
{
    public interface ILoggingService
    {
        Task SendLogMessageAsync(LoggingModel loggingModel);
    }
}
