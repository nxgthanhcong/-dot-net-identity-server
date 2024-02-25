using Core.Models.ResponseModels;
using Security.Models.ProcessModels;
using Security.Models.ResponseModels;

namespace Security.Business.Interfaces
{
    public interface ISecurityBusiness
    {
        Task<ResponseModel> Signup(UserModel user);
        Task<ResponseModel> Signin(UserModel user);
        Task<ResponseModel> RefreshToken(TokenRes tokenModel);
        Task<ResponseModel> Normal();
    }
}