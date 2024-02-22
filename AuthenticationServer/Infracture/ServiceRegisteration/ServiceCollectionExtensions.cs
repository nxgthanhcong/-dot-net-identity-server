using AuthenticationServer.Infracture.AutoMapper;
using Core.DatabaseProviders.Implementions.Postgre;
using Core.DatabaseProviders.Interfaces;
using Microsoft.Extensions.Configuration;
using Security.Business.Implementions;
using Security.Business.Interfaces;
using Security.Repositories.Implementions;
using Security.Repositories.Interfaces;
using System.Runtime.CompilerServices;

namespace AuthenticationServer.Infracture.ServiceRegisteration
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInternalServices(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

            services.AddScoped<ISecurityRepository, SecurityRepository>();
            services.AddScoped<ISecurityBusiness, SecurityBusiness>();
            services.AddScoped<IDbProvider, PostgreProvider>();

            services.Configure<PostgreConnectConfig>(configuration.GetSection("PostgreConnectConfig"));

            return services;
        }
    }
}
