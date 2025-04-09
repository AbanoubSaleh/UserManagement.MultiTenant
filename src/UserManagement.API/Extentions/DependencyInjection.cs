using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using UserManagement.Application.Common.Interfaces;
using UserManagement.Infrastructure.Services;
using UserManagement.Application.Common.Interfaces.Repositories;
using UserManagement.Infrastructure.Repositories;

namespace UserManagement.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            // Register HttpContextAccessor
            services.AddHttpContextAccessor();
            
            // Register CurrentUserService
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddTransient<ITenantRepository, TenantRepository>();
            services.AddTransient<ITenantContext, TenantContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IDateTime, DateTimeService>();
            return services;
        }
    }
}