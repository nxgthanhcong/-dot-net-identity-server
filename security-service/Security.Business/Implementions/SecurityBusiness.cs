using Core.Logging.Interfaces;
using Core.Models.LoggingModels;
using Core.Models.ResponseModels;
using Core.Utilities;
using Security.Business.Interfaces;
using Security.Models.ProcessModels;
using Security.Models.ResponseModels;
using Security.Repositories.Interfaces;
using Security.Services.Interfaces;
using System.Security.Claims;

namespace Security.Business.Implementions
{
    public class SecurityBusiness : ISecurityBusiness
    {
        private readonly ITokenService tokenService;
        private readonly ISecurityRepository securityRepository;
        private readonly ILoggingService loggingService;

        public SecurityBusiness(ISecurityRepository securityRepository, ITokenService tokenService, ILoggingService loggingService)
        {
            this.tokenService = tokenService;
            this.securityRepository = securityRepository;
            this.loggingService = loggingService;
        }

        public async Task<ResponseModel> Signup(UserModel user)
        {
            try
            {
                UserModel userInDb = await securityRepository.GetUserByUsername(user.Username);
                if(userInDb != null)
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

        public async Task<ResponseModel> Signin(UserModel user)
        {
            try
            {
                UserModel userInDb = await securityRepository.GetUserByUsername(user.Username);
                if (userInDb == null)
                {
                    return ResponseModel.Failed("user not exist");
                }

                bool isValidPassword = PasswordHasher.VerifyPassword(user.Password, userInDb.Password);
                if(!isValidPassword)
                {
                    return ResponseModel.Failed("wrong password");
                }

                return ResponseModel.Succeed(new TokenRes
                {
                    Token = tokenService.GenerateToken(user),
                    RefreshToken = tokenService.GenerateRefreshToken(user),
                    ExpireTime = DateTime.Now.AddMinutes(1)
                });
            }
            catch (Exception ex)
            {
                return ResponseModel.Error;
            }
        }
    
        public async Task<ResponseModel> RefreshToken(TokenRes tokenModel)
        {
            try
            {
                bool isValidRefeshToken = tokenService.VerifyRefreshToken(tokenModel.RefreshToken, out ClaimsPrincipal principal);
                if (!isValidRefeshToken)
                {
                    return ResponseModel.Failed("refresh token is not valid");
                }

                var usernameClaim = principal.FindFirst("username")?.Value;
                if (string.IsNullOrEmpty(usernameClaim))
                {
                    return ResponseModel.Failed("claims do not have username");
                }

                UserModel userInDb = await securityRepository.GetUserByUsername(usernameClaim);
                if (userInDb == null)
                {
                    return ResponseModel.Failed("user not exist");
                }

                return ResponseModel.Succeed(new TokenRes
                {
                    Token = tokenService.GenerateToken(userInDb),
                    RefreshToken = tokenService.GenerateRefreshToken(userInDb),
                    ExpireTime = DateTime.Now.AddMinutes(1)
                });
            }
            catch (Exception ex)
            {
                return ResponseModel.Error;
            }
        }
    
        public async Task Normal()
        {
            loggingService.SendLogMessageAsync(new LoggingModel
            {
                LogLevel = LogLevelEnum.Info,
                Source = "dev",
                Message = "success",
            });
        }
    }
}
