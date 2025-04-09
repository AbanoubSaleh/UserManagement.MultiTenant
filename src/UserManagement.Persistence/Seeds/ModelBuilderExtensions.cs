using Microsoft.EntityFrameworkCore;

namespace UserManagement.Persistence.Seeds
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.SeedTenants();
            // Add other seed methods here as needed
        }
    }
}