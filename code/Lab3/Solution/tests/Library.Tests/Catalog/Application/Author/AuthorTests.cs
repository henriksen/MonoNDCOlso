using Library.Catalog.Application.Author;
using Library.Catalog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Library.Tests.Catalog.Application.Author
{
    public class AuthorTests
    {
        [Test]
        public async Task Create_ShouldSaveAuthorToDatabase()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<CatalogDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            await using var context = new CatalogDbContext(options);
            var sut = new Create.Handler(context);

            var command = new Create.Command("Test Author", "Biography", DateTimeOffset.Now.AddYears(-30), null);

            // Act
            var authorId = await sut.Handle(command, CancellationToken.None);

            // Assert
            var savedAuthor = await context.Authors.FindAsync(authorId);
            Assert.That(savedAuthor, Is.Not.Null);
            Assert.That(savedAuthor!.Name, Is.EqualTo("Test Author"));
        }
    }
}