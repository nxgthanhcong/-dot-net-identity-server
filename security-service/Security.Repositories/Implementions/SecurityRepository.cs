using Core.DatabaseProviders.Interfaces;
using Security.Models.RequestModels;
using Security.Repositories.Interfaces;

namespace Security.Repositories.Implementions
{
    public class SecurityRepository : ISecurityRepository
    {
        private readonly IDbProvider dbProvider;
        public SecurityRepository(IDbProvider dbProvider) 
        { 
            this.dbProvider = dbProvider;
        }

        public async Task<bool> CreateUser(ManualSignupReq manualSignupReq)
        {
            string sql = "INSERT INTO users (username, password) VALUES (@Username, @Password)";
            return await dbProvider.ExcuteAsync(sql, manualSignupReq);
        }
    }
}
