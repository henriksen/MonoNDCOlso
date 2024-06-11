using Library.Catalog.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Library.Catalog.Infrastructure.Data
{
    public class CatalogDbContext : DbContext
    {
        private readonly IMediator? _mediator;
        public CatalogDbContext(DbContextOptions<CatalogDbContext> options, IMediator? mediator) : base(options)
        {
            _mediator = mediator;
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder
                .Properties<AuthorId>()
                .HaveConversion<AuthorId.EfCoreValueConverter>();
            configurationBuilder
                .Properties<BookId>()
                .HaveConversion<BookId.EfCoreValueConverter>();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Catalog");

            modelBuilder.Entity<Domain.Author>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<Domain.Book>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            if (_mediator != null)
            {
                await _mediator.DispatchDomainEventsAsync(this);
            }
            return await base.SaveChangesAsync(cancellationToken);
        }

        public DbSet<Domain.Author> Authors { get; set; } = null!;
        public DbSet<Domain.Book> Books { get; set; } = null!;
    }
}