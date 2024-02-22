namespace Core.DatabaseProviders.Interfaces
{
    public interface IDbProvider
    {
        Task<bool> ExcuteAsync(string sql, object param);
        Task<List<T>> QueryAsync<T>(string sql, object param);
    }
}
