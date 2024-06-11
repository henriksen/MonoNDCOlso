using Library.Catalog.Application.Author.EventHandlers;
using Library.Catalog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Tests.Catalog.Application.Integration;

public class IntegrationTestsBase
{
    protected ServiceCollection Services;

    [SetUp]
    public void SetUp()
    {
        Services = new ServiceCollection();
        Services.AddDbContext<CatalogDbContext>(options => options.UseInMemoryDatabase(Guid.NewGuid().ToString()));
        Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(UpdateAuthorBooksOnBookAddedEvent).Assembly);
        });
    }
}