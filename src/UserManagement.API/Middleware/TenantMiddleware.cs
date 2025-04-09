using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using UserManagement.Application.Common.Interfaces;
using UserManagement.Application.Common.Interfaces.Repositories;

namespace UserManagement.Infrastructure.Middleware
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;

        // Remove ITenantRepository from constructor
        public TenantMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ITenantContext tenantContext, IServiceProvider serviceProvider)
        {
            // Skip tenant validation for Swagger requests
            if (context.Request.Path.StartsWithSegments("/swagger"))
            {
                await _next(context);
                return;
            }

            if (!context.Request.Headers.TryGetValue("X-Tenant-Id", out var tenantIdHeader))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(new { error = "Tenant ID is required" });
                return;
            }

            if (!Guid.TryParse(tenantIdHeader, out var tenantId))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(new { error = "Invalid Tenant ID format" });
                return;
            }

            using (var scope = serviceProvider.CreateScope())
            {
                var tenantRepository = scope.ServiceProvider.GetRequiredService<ITenantRepository>();
                var cache = scope.ServiceProvider.GetRequiredService<IMemoryCache>();

                var activeTenantIds = await GetActiveTenantIds(tenantRepository, cache);
                if (!activeTenantIds.Contains(tenantId))
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsJsonAsync(new { error = "Invalid or inactive tenant" });
                    return;
                }

                tenantContext.TenantId = tenantId;
            }

            await _next(context);
        }

        private async Task<HashSet<Guid>> GetActiveTenantIds(ITenantRepository tenantRepository, IMemoryCache cache)
        {
            const string TENANT_CACHE_KEY = "ActiveTenants";
            if (!cache.TryGetValue(TENANT_CACHE_KEY, out HashSet<Guid> tenantIds))
            {
                var tenants = await tenantRepository.GetAllAsync();
                tenantIds = new HashSet<Guid>(
                    tenants.Where(t => t.IsActive && !t.IsDeleted)
                           .Select(t => t.Id)
                );

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromHours(1));

                cache.Set(TENANT_CACHE_KEY, tenantIds, cacheOptions);
            }

            return tenantIds;
        }
    }
}