using Microsoft.AspNetCore.Http;
using UserManagement.Application.Common.Interfaces;

namespace UserManagement.Infrastructure.Services
{
    public class TenantContext : ITenantContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private Guid? _cachedTenantId;

        public TenantContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid TenantId
        {
            get
            {
                if (_cachedTenantId.HasValue)
                    return _cachedTenantId.Value;

                var tenantIdHeader = _httpContextAccessor.HttpContext?.Request?.Headers["X-Tenant-Id"].FirstOrDefault();
                if (!string.IsNullOrEmpty(tenantIdHeader) && Guid.TryParse(tenantIdHeader, out var tenantId))
                {
                    _cachedTenantId = tenantId;
                    return tenantId;
                }

                _cachedTenantId = Guid.Empty;
                return Guid.Empty;
            }
            set => _cachedTenantId = value; // Added setter
        }

        public string TenantCode => 
            _httpContextAccessor.HttpContext?.Request?.Headers["X-Tenant-Code"].FirstOrDefault() ?? string.Empty;

        public bool IsValid => TenantId != Guid.Empty && !string.IsNullOrEmpty(TenantCode);
    }
}