using Core.Models.ResponseModels;
using Core.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Security.Business.Interfaces;
using Security.Models.ProcessModels;
using Security.Models.RequestModels;
using Security.Repositories.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Security.Business.Implementions
{
    public class SecurityBusiness : ISecurityBusiness
    {
        private readonly ISecurityRepository securityRepository;

        private readonly string Issuer;
        private readonly string Audience;
        private readonly string Key;

        public SecurityBusiness(ISecurityRepository securityRepository, IConfiguration configuration)
        {
            this.securityRepository = securityRepository;
            Issuer = configuration["TokenValidationParameters:ValidIssuer"];
            Audience = configuration["TokenValidationParameters:ValidAudience"];
            Key = configuration["TokenValidationParameters:Key"];
        }

        private string GenerateToken(UserModel user)
        {
            // Create claims for the token
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("username", user.Username),
                new Claim("role", user.Role ?? string.Empty)
            };

            // Create a security key using the secret key
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Key));

            // Create credentials using the security key
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create a JWT token
            var token = new JwtSecurityToken(
                issuer: Issuer,
                audience: Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(5), // Token expiration time
                signingCredentials: creds
            );

            // Serialize the token to a string
            return new JwtSecurityTokenHandler().WriteToken(token);
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

                string token = GenerateToken(user);

                return ResponseModel.Succeed(token);
            }
            catch (Exception ex)
            {
                return ResponseModel.Error;
            }
        }
    }
}
