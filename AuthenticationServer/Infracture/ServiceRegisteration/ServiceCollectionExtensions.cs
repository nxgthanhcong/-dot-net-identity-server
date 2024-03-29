﻿using AuthenticationServer.Infracture.AutoMapper;
using Core.DatabaseProviders.Implementions.Postgre;
using Core.DatabaseProviders.Interfaces;
using Core.HttpServices.Implementions;
using Core.HttpServices.Interfaces;
using Core.Logging.Implementions;
using Core.Logging.Interfaces;
using Core.NotifyService.Implementions;
using Core.NotifyService.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Security.Business.Implementions;
using Security.Business.Interfaces;
using Security.Repositories.Implementions;
using Security.Repositories.Interfaces;
using Security.Services.Implementions;
using Security.Services.Interfaces;
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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenValidationParameters:Key"])),
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddScoped<ILoggingService, ElasticLoggingService>();
            services.AddScoped<INotifyService, TelegramNotifyService>();
            services.AddScoped<IHttpService, HttpService>();

            services.AddScoped<ISecurityRepository, SecurityRepository>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ISecurityBusiness, SecurityBusiness>();
            services.AddScoped<IDbProvider, PostgreProvider>();

            services.Configure<PostgreConnectConfig>(configuration.GetSection("PostgreConnectConfig"));

            return services;
        }
    }
}
