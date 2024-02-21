using Security.Models.RequestModels;

namespace Security.Repositories.Interfaces
{
    public interface ISecurityRepository
    {
        Task<bool> CreateUser(ManualSignupReq manualSignupReq);
    }
}
