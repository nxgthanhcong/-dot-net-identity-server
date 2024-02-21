using Microsoft.AspNetCore.Mvc;
using Security.Business.Interfaces;
using Security.Models.RequestModels;

namespace AuthenticationServer.Controllers.v1
{
    [Route("api/v1/security")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private readonly ISecurityBusiness securityBusiness;
        public SecurityController(ISecurityBusiness securityBusiness)
        {
            this.securityBusiness = securityBusiness;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup(ManualSignupReq manualSignupReq)
        {
            return Ok(await securityBusiness.Signup(manualSignupReq));
        }
    }
}
