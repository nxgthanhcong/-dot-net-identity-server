using AutoMapper;
using Core.Utilities;
using Microsoft.AspNetCore.Mvc;
using Security.Business.Interfaces;
using Security.Models.ProcessModels;
using Security.Models.RequestModels;

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
    }
}
