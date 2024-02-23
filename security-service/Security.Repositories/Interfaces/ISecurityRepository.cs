using Security.Models.ProcessModels;

namespace Security.Repositories.Interfaces
{
    public interface ISecurityRepository
    {
        Task<UserModel> GetUserByUsername(string username);
        Task<bool> CreateUser(UserModel user);
    }
}
