namespace UserManagement.Domain.Entities
{
    public class UserPermission
    {
        public Guid UserId { get; set; }
        public Guid PermissionId { get; set; }
        public virtual User User { get; set; }
        public virtual Permission Permission { get; set; }
    }
}