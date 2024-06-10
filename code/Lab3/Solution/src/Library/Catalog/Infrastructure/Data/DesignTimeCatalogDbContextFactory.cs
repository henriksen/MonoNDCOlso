using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Library.Catalog.Infrastructure.Data
{
    public class DesignTimeCatalogDbContextFactory : IDesignTimeDbContextFactory<CatalogDbContext>
    {
        public CatalogDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CatalogDbContext>();
            optionsBuilder.UseSqlServer("Server=localhost;Database=Library;User Id=sa;Password=Password_123#;TrustServerCertificate=True");

            return new CatalogDbContext(optionsBuilder.Options);
        }
    }
}