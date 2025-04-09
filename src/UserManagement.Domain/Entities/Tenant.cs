using UserManagement.Domain.Common;

namespace UserManagement.Domain.Entities
{
    public class Tenant : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
    }
}