using Microsoft.AspNetCore.Identity;

namespace UserManagement.Domain.Entities
{
    public class Role : IdentityRole<Guid>
    {
        public Guid TenantId { get; set; }
        public virtual Tenant Tenant { get; set; }
        public virtual ICollection<RolePermission> RolePermissions { get; set; }
    }
}