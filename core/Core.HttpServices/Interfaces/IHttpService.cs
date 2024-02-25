namespace Core.HttpServices.Interfaces
{
    public interface IHttpService
    {
        Task<HttpResponseMessage> PostAsync(string url, object contentObj);
    }
}
