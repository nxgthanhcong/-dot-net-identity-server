using Core.Logging.Interfaces;
using Core.Models.LoggingModels;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;

namespace Core.Logging.Implementions
{
    public class ElasticLoggingService : ILoggingService
    {
        private readonly string url;
        public ElasticLoggingService(IConfiguration configuration) 
        {
            url = $"{configuration["ElasticSearchIndex:Host"]}{configuration["ElasticSearchIndex:Index"]}/_doc";
        }
        public async Task SendLogMessageAsync(LoggingModel loggingModel)
        {
            using (var httpClient = new HttpClient())
            {
                var content = new StringContent(JsonSerializer.Serialize(loggingModel), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Log message indexed successfully.");
                }
                else
                {
                    Console.WriteLine($"Failed to index log message: {response.StatusCode}");
                }
            }
        }
    }
}
