using Microsoft.AspNetCore.Identity;
using UserManagement.Domain.Common;

namespace UserManagement.Domain.Entities
{
    public class User : IdentityUser<Guid>
    {
        public Guid TenantId { get; set; }
        public virtual Tenant Tenant { get; set; }
        public virtual ICollection<UserPermission> UserPermissions { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
    }
}