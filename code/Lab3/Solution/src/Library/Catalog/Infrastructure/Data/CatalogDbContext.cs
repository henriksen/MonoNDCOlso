using Library.Catalog.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Library.Catalog.Infrastructure.Data
{
    public class CatalogDbContext : DbContext
    {
        public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options) { }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder
                .Properties<AuthorId>()
                .HaveConversion<AuthorId.EfCoreValueConverter>();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Catalog");

            modelBuilder.Entity<Domain.Author>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
            });
        }

        public DbSet<Domain.Author> Authors { get; set; }
    }
}