using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using UserManagement.Application.Common.Interfaces;

namespace UserManagement.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public Guid? UserId
        {
            get
            {
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
                return !string.IsNullOrEmpty(userId) ? Guid.Parse(userId) : null;
            }
        }

        public string UserName
        {
            get
            {
                return _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                return _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
            }
        }
    }
}