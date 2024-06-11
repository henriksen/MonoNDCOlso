using Library.Catalog.Application.Author;
using Library.Catalog.Application.Author.EventHandlers;
using Library.Catalog.Contracts;
using Library.Catalog.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Tests.Catalog.Application.Integration;

[TestFixture]
internal class BookUpdatesAuthorTests
{
    private IServiceProvider _serviceProvider;

    [TearDown]
    public void TearDown()
    {
        if (_serviceProvider is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }

    [SetUp]
    public void SetUp()
    {
        var services = new ServiceCollection();
        services.AddDbContext<CatalogDbContext>(options => options.UseInMemoryDatabase(Guid.NewGuid().ToString()));
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(UpdateAuthorBooksOnBookAddedEvent).Assembly);
        });
        _serviceProvider = services.BuildServiceProvider();
    }

    [Test]
    public async Task BookCreate_WithAuthor_ShouldUpdateAuthorBooks()
    {
        // Arrange
        var mediator = _serviceProvider.GetRequiredService<IMediator>();
        var command = new Create.Command("Test Author", "Biography", DateTimeOffset.Now.AddYears(-30), null);
        var authorId = await mediator.Send(command);

        var createBookCommand = new Library.Catalog.Application.Book.Create.Command(
            "Test Book",
            "1234567890123",
            authorId.Value,
            PublisherId.New(),
            DateTimeOffset.Now,
            GenreId.New(),
            "Description");

        // Act
        var bookId = await mediator.Send(createBookCommand);

        // Assert
        var context = _serviceProvider.GetRequiredService<CatalogDbContext>();
        var author = await context.Authors.FirstOrDefaultAsync(a => a.Id == authorId.Value);

        Assert.IsNotNull(author);
        Assert.That(author!.Books.Count, Is.EqualTo(1));
        Assert.That(author.Books.First(), Is.EqualTo(bookId.Value));
    }
}