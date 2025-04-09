using Microsoft.EntityFrameworkCore;
using UserManagement.Application.Common.Interfaces.Repositories;
using UserManagement.Domain.Entities;
using UserManagement.Persistence.Context;

namespace UserManagement.Infrastructure.Repositories
{
    public class TenantRepository : ITenantRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Tenant> _tenants;

        public TenantRepository(ApplicationDbContext context)
        {
            _context = context;
            _tenants = context.Set<Tenant>();
        }

        public async Task<Tenant?> GetByIdAsync(Guid id)
        {
            return await _tenants.FindAsync(id);
        }

        public async Task<IEnumerable<Tenant>> GetAllAsync()
        {
            return await _tenants.ToListAsync();
        }

        public async Task<IEnumerable<Tenant>> GetActiveTenants()
        {
            return await _tenants
                .Where(t => t.IsActive && !t.IsDeleted)
                .ToListAsync();
        }

        public async Task<Tenant> AddAsync(Tenant tenant)
        {
            await _tenants.AddAsync(tenant);
            return tenant;
        }

        public void Update(Tenant tenant)
        {
            _tenants.Update(tenant);
        }

        public void Delete(Tenant tenant)
        {
            _tenants.Remove(tenant);
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _tenants.AnyAsync(t => t.Id == id);
        }

        public async Task<Tenant?> GetByCodeAsync(string code)
        {
            return await _tenants
                .FirstOrDefaultAsync(t => t.Code == code && t.IsActive && !t.IsDeleted);
        }
    }
}