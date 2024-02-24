using Security.Models.ProcessModels;
using System.Security.Claims;

namespace Security.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(UserModel user);
        string GenerateRefreshToken(UserModel user);
        bool VerifyRefreshToken(string refreshToken, out ClaimsPrincipal claimsPrincipal);
    }
}
