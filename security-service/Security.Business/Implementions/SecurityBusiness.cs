using Core.Models.ResponseModels;
using Core.Utilities;
using Security.Business.Interfaces;
using Security.Models.RequestModels;
using Security.Repositories.Interfaces;

namespace Security.Business.Implementions
{
    public class SecurityBusiness : ISecurityBusiness
    {
        private readonly ISecurityRepository securityRepository;
        public SecurityBusiness(ISecurityRepository securityRepository)
        {
            this.securityRepository = securityRepository;
        }

        public async Task<ResponseModel> Signup(ManualSignupReq manualSignupReq)
        {
            try
            {
                bool rs = await securityRepository.CreateUser(manualSignupReq);
                return ResponseModel.Succeed(rs);
            }
            catch (Exception ex)
            {
                return ResponseModel.Error;
            }
        }
    }
}
