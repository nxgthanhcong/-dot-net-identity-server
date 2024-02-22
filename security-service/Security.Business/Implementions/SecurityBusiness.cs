using Core.Models.ResponseModels;
using Core.Utilities;
using Security.Business.Interfaces;
using Security.Models.ProcessModels;
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

        public async Task<ResponseModel> Signup(UserModel user)
        {
            try
            {
                bool isExistInDb = await securityRepository.IsExistUserInDb(user.Username);
                if(isExistInDb)
                {
                    return ResponseModel.Failed("username already exist");
                }

                user.PasswordHash = PasswordHasher.HashPassword(user.Password);

                bool rs = await securityRepository.CreateUser(user);
                return ResponseModel.Succeed(rs);
            }
            catch (Exception ex)
            {
                return ResponseModel.Error;
            }
        }
    }
}
