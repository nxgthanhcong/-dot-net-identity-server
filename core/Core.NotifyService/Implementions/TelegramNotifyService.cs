using Core.HttpServices.Interfaces;
using Core.Models.LoggingModels;
using Core.NotifyService.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace Core.NotifyService.Implementions
{
    public class TelegramNotifyService : INotifyService
    {
        private readonly IHttpService httpService;
        private readonly IConfiguration configuration;
        private readonly string apiUrl;
        public TelegramNotifyService(IHttpService httpService, IConfiguration configuration)
        {
            this.httpService = httpService;
            this.configuration = configuration;

            string botToken = configuration["TelegramNotifyBot:Token"];
            apiUrl = $"https://api.telegram.org/bot{botToken}/sendMessage";
        }

        public async Task SendNotiMessageAsync(LoggingModel loggingModel)
        {
            string chatId = configuration["TelegramNotifyBot:ChatId"];

            var responseAwait = httpService.PostAsync(apiUrl, new
            {
                chat_id = chatId,
                text = JsonSerializer.Serialize(loggingModel)
            });
        }

        public async Task SendNotiMessageAsync(string message)
        {
            string chatId = configuration["TelegramNotifyBot:ChatId"];

            var responseAwait = httpService.PostAsync(apiUrl, new
            {
                chat_id = chatId,
                text = JsonSerializer.Serialize(message)
            });
        }
    }
}
