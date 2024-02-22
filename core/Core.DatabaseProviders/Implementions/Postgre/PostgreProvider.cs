using Core.DatabaseProviders.Interfaces;
using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Data;

namespace Core.DatabaseProviders.Implementions.Postgre
{
    public class PostgreProvider : IDbProvider
    {
        private readonly PostgreConnectConfig _configuration;
        private readonly string connectionString;
        public PostgreProvider(IOptions<PostgreConnectConfig> configuration)
        {
            _configuration = configuration.Value;
            connectionString = string.Format("Host={0};Port={1};Username={2};Password={3};Database={4}", _configuration.Host, _configuration.Port, _configuration.Username, _configuration.Password, _configuration.Database);
        }

        public async Task<bool> ExcuteAsync(string sql, object param)
        {
            bool rs = false;
            using (IDbConnection dbConnection = new NpgsqlConnection(connectionString))
            {
                dbConnection.Open();

                rs = await dbConnection.ExecuteAsync(sql, param) > 0;

                dbConnection.Close();
            }
            return rs;
        }

        public async Task<List<T>> QueryAsync<T>(string sql, object param)
        {
            using (IDbConnection dbConnection = new NpgsqlConnection(connectionString))
            {
                return (await dbConnection.QueryAsync<T>(sql, param)).ToList();
            }
        }
    }
}