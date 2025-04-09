using Microsoft.EntityFrameworkCore;
using System;
using UserManagement.Domain.Entities;

namespace UserManagement.Persistence.Seeds
{
    internal static class TenantSeed
    {
        private static readonly DateTime _baseDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static void SeedTenants(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tenant>().HasData(
                new Tenant
                {
                    Id = Guid.Parse("5e95cac8-4c4c-4b22-b5e2-d5b0e6b7c9e2"),
                    Name = "Saint George Church",
                    Code = "SGC",
                    IsActive = true,
                    CreatedAt = _baseDate,
                    UpdatedAt = null,
                    IsDeleted = false,
                    DeletedAt = null
                },
                new Tenant
                {
                    Id = Guid.Parse("8e95cac8-4c4c-4b22-b5e2-d5b0e6b7c9e3"),
                    Name = "Mediansta",
                    Code = "MED",
                    IsActive = true,
                    CreatedAt = _baseDate,
                    UpdatedAt = null,
                    IsDeleted = false,
                    DeletedAt = null
                }
            );
        }
    }
}