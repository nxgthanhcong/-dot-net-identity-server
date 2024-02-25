using Core.HttpServices.Interfaces;
using System.Text.Json.Nodes;
using System.Text;
using System.Text.Json;

namespace Core.HttpServices.Implementions
{
    public class HttpService : IHttpService
    {
        public async Task<HttpResponseMessage> PostAsync(string apiUrl, object contentObj)
        {
            string jsonBody = JsonSerializer.Serialize(contentObj);
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.PostAsync(apiUrl, new StringContent(jsonBody, Encoding.UTF8, "application/json"));

                return response;
            }
        }
    }
}
