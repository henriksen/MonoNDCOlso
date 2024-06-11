using Library.Catalog.Application.Author;
using Library.Catalog.Contracts;
using Library.Catalog.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Tests.Catalog.Application.Integration;

[TestFixture]
internal class BookUpdatesAuthorTests : IntegrationTestsBase
{
    [Test]
    public async Task BookCreate_WithAuthor_ShouldUpdateAuthorBooks()
    {
        // Arrange
        var serviceProvider = Services.BuildServiceProvider();

        var mediator = serviceProvider.GetRequiredService<IMediator>();
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
        var context = serviceProvider.GetRequiredService<CatalogDbContext>();
        var author = await context.Authors.FirstOrDefaultAsync(a => a.Id == authorId.Value);

        Assert.That(author, Is.Not.Null);
        Assert.That(author!.Books.Count, Is.EqualTo(1));
        Assert.That(author.Books.First(), Is.EqualTo(bookId.Value));
    }
}