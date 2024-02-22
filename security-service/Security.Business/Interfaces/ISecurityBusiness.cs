using Core.Models.ResponseModels;
using Security.Models.ProcessModels;

namespace Security.Business.Interfaces
{
    public interface ISecurityBusiness
    {
        Task<ResponseModel> Signup(UserModel user);
    }
}