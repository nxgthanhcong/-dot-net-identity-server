using Core.DatabaseProviders.Interfaces;
using Security.Models.ProcessModels;
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

        public async Task<bool> IsExistUserInDb(string username)
        {
            string sql = "select * from identity.public.users where username = @username";
            var rs = await dbProvider.QueryAsync<ManualSignupReq>(sql, new
            {
                @username = username
            });
            return rs.Any();
        }

        public async Task<bool> CreateUser(UserModel user)
        {
            string sql = "INSERT INTO users (username, password) VALUES (@Username, @PasswordHash)";
            return await dbProvider.ExcuteAsync(sql, user);
        }
    }
}
