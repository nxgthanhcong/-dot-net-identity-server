using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Security.Models.ProcessModels;
using Security.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Security.Services.Implementions
{
    public class TokenService : ITokenService
    {
        private readonly string RefreshKey;
        private readonly string Issuer;
        private readonly string Audience;
        private readonly string Key;
        public TokenService(IConfiguration configuration)
        {
            RefreshKey = configuration["TokenValidationParameters:RefreshKey"];
            Issuer = configuration["TokenValidationParameters:ValidIssuer"];
            Audience = configuration["TokenValidationParameters:ValidAudience"];
            Key = configuration["TokenValidationParameters:Key"];
        }

        public string GenerateRefreshToken(UserModel user)
        {
            var claims = new[]
            {
                new Claim("username", user.Username),
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(RefreshKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(60), 
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateToken(UserModel user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("username", user.Username),
                new Claim("role", user.Role ?? string.Empty)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Key));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: Issuer,
                audience: Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(1), 
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool VerifyRefreshToken(string refreshToken, out ClaimsPrincipal claimsPrincipal)
        {
            claimsPrincipal = new ClaimsPrincipal();

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(RefreshKey);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
            };

            SecurityToken validatedToken;
            var principal = tokenHandler.ValidateToken(refreshToken, tokenValidationParameters, out validatedToken);

            if (principal == null)
            {
                return false;
            }

            claimsPrincipal = principal; //.FindFirst("username")?.Value;
            return true;
        }
    }
}
