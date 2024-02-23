using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Core.Utilities
{
    public static class JwtTokenResolver
    {
        // Secret key for JWT token generation (should be securely stored)
        private const string SecretKey = "this_is_best_secu_kehis_is_best_secu_key";
        private const string Issuer = "AuthenticationServerOfDeadC";
        private const string Audience = "Application1";

        public static string GenerateToken(string username)
        {
            // Create claims for the token
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("username", username),
                //new Claim("role", "admin")
            };

            // Create a security key using the secret key
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(SecretKey));

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

        public static ClaimsPrincipal VerifyToken(string token)
        {
            try
            {
                // Create parameters for token validation
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Issuer, // Should match the issuer used during token generation
                    ValidAudience = Audience, // Should match the audience used during token generation
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(SecretKey)) // Security key for token validation
                };

                // Validate the token and retrieve claims
                var tokenHandler = new JwtSecurityTokenHandler();
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);

                // Check if the token is a JWT token
                if (!(validatedToken is JwtSecurityToken jwtToken))
                {
                    throw new SecurityTokenException("Invalid token");
                }

                // Optionally, you can perform additional validation or processing here

                return principal;
            }
            catch (Exception ex)
            {
                // Token validation failed
                Console.WriteLine($"Token validation failed: {ex.Message}");
                return null;
            }
        }
    }
}
