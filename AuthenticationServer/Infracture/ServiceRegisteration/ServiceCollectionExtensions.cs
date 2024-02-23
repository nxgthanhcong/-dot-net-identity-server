using AuthenticationServer.Infracture.AutoMapper;
using Core.DatabaseProviders.Implementions.Postgre;
using Core.DatabaseProviders.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Security.Business.Implementions;
using Security.Business.Interfaces;
using Security.Repositories.Implementions;
using Security.Repositories.Interfaces;
using System.Runtime.CompilerServices;
using System.Text;

namespace AuthenticationServer.Infracture.ServiceRegisteration
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInternalServices(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["TokenValidationParameters:ValidIssuer"],
                    ValidAudience = configuration["TokenValidationParameters:ValidAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenValidationParameters:Key"]))
                };
            });

            services.AddScoped<ISecurityRepository, SecurityRepository>();
            services.AddScoped<ISecurityBusiness, SecurityBusiness>();
            services.AddScoped<IDbProvider, PostgreProvider>();

            services.Configure<PostgreConnectConfig>(configuration.GetSection("PostgreConnectConfig"));

            return services;
        }
    }
}
