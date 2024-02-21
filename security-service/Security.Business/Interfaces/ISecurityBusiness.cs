using Core.Models.ResponseModels;
using Security.Models.RequestModels;

namespace Security.Business.Interfaces
{
    public interface ISecurityBusiness
    {
        Task<ResponseModel> Signup(ManualSignupReq manualSignupReq);
    }
}