namespace UserManagement.Domain.Entities
{
    public class Permission
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public Guid TenantId { get; set; }
        public virtual Tenant Tenant { get; set; }
        public virtual ICollection<UserPermission> UserPermissions { get; set; }
        public virtual ICollection<RolePermission> RolePermissions { get; set; }
    }
}