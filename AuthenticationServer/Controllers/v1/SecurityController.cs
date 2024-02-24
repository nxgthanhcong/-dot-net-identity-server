using AutoMapper;
using Core.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Security.Business.Interfaces;
using Security.Models.ProcessModels;
using Security.Models.RequestModels;
using Security.Models.ResponseModels;

namespace AuthenticationServer.Controllers.v1
{
    [Route("api/v1/security")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ISecurityBusiness securityBusiness;
        public SecurityController(ISecurityBusiness securityBusiness, IMapper mapper)
        {
            this.securityBusiness = securityBusiness;
            this.mapper = mapper;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup(ManualSignupReq manualSignupReq)
        {
            UserModel user = mapper.Map<UserModel>(manualSignupReq);

            return Ok(await securityBusiness.Signup(user));
        }

        [HttpPost("signin")]
        public async Task<IActionResult> Signin(ManualSignupReq manualSignupReq)
        {
            manualSignupReq.Username = "admin@example.com";
            manualSignupReq.Password = "admin@123";
            UserModel user = mapper.Map<UserModel>(manualSignupReq);

            return Ok(await securityBusiness.Signin(user));
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenRes tokenRes)
        {
            return Ok(await securityBusiness.RefreshToken(tokenRes));
        }

        [HttpGet("normal")]
        public async Task<IActionResult> Normal()
        {
            securityBusiness.Normal();
            return Ok("abc");
        }

        [HttpGet("auth")]
        [Authorize]
        public async Task<IActionResult> Auth()
        {
            return Ok("def");
        }
    }
}
