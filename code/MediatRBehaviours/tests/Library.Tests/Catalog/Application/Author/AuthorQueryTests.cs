using Library.Catalog.Application.Author;
using Library.Catalog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Library.Tests.Catalog.Application.Author
{
    public class AuthorQueryTests
    {
        [Test]
        public async Task GetList_ShouldReturnAuthors()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<CatalogDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            await using var context = new CatalogDbContext(options, null);

            var createHandler = new Create.Handler(context);
            var sut = new GetList.Handler(context);

            var author1 = new Create.Command("Author One", "Biography One", DateTimeOffset.Now.AddYears(-40), null);
            var author2 = new Create.Command("Author Two", "Biography Two", DateTimeOffset.Now.AddYears(-30), null);

            await createHandler.Handle(author1, CancellationToken.None);
            await createHandler.Handle(author2, CancellationToken.None);

            var query = new GetList.Query(1, 10);
            context.ChangeTracker.Clear();

            // Act
            var result = await sut.Handle(query, CancellationToken.None);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.TotalCount, Is.EqualTo(2));
                Assert.That(result.Authors.Count, Is.EqualTo(2));
                Assert.That(result.Authors[0].Name, Is.EqualTo("Author One"));
                Assert.That(result.Authors[1].Name, Is.EqualTo("Author Two"));
            });
        }
    }
}