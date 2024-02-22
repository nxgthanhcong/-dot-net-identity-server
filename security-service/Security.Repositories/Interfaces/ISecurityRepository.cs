using Security.Models.ProcessModels;

namespace Security.Repositories.Interfaces
{
    public interface ISecurityRepository
    {
        Task<bool> CreateUser(UserModel user);
        Task<bool> IsExistUserInDb(string username);
    }
}
