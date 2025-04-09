using UserManagement.Domain.Entities;

namespace UserManagement.Application.Common.Interfaces.Repositories
{
    public interface ITenantRepository
    {
        Task<Tenant> GetByIdAsync(Guid id);
        Task<IEnumerable<Tenant>> GetAllAsync();
        Task<IEnumerable<Tenant>> GetActiveTenants();
        Task<Tenant> AddAsync(Tenant tenant);
        void Update(Tenant tenant);
        void Delete(Tenant tenant);
        Task<bool> ExistsAsync(Guid id);
        Task<Tenant> GetByCodeAsync(string code);
    }
}