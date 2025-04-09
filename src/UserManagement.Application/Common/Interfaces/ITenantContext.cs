namespace UserManagement.Application.Common.Interfaces
{
    public interface ITenantContext
    {
        /// <summary>
        /// Gets or sets the current tenant's unique identifier.
        /// </summary>
        Guid TenantId { get; set; }

        /// <summary>
        /// Gets the current tenant's code.
        /// </summary>
        string TenantCode { get; }

        /// <summary>
        /// Indicates whether the tenant context contains valid tenant information.
        /// </summary>
        bool IsValid { get; }
    }
}