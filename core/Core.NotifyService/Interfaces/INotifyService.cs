using Core.Models.LoggingModels;

namespace Core.NotifyService.Interfaces
{
    public interface INotifyService
    {
        Task SendNotiMessageAsync(LoggingModel loggingModel);
        Task SendNotiMessageAsync(string message);
    }
}
