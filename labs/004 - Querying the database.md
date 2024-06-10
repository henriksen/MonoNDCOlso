# Lab: Querying for Authors

In this lab, you will create a new MediatR handler to query for authors with paging support. You will also write a test that uses the `Create` handler to add two authors and verifies that they are returned by the query.

## Steps

### 1. Create the GetList Handler

#### Using Command Line/VS Code

1. Create a new file named `GetList.cs` in the `Catalog/Application/Author` folder:

    ```bash
    touch src/Library/Catalog/Application/Author/GetList.cs
    ```

#### Using Visual Studio

1. Right-click on the **Author** folder in **Solution Explorer**.
2. Select **Add** > **Class**.
3. Name the class `GetList` and click **Add**.

#### Add Code for GetList Handler

Open the `GetList.cs` file and add the following code:

```csharp
using Library.Catalog.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Library.Catalog.Application.Author
{
    public class GetList
    {
        public record Query(int PageNumber, int PageSize) : IRequest<Response>;

        public record AuthorResponse(string Id, string Name, string Biography, DateTimeOffset? BirthDate, DateTimeOffset? DeathDate);

        public record Response(int TotalCount, List<AuthorResponse> Authors);

        public class Handler(CatalogDbContext context) : IRequestHandler<Query, Response>
        {
            public async Task<Response> Handle(Query query, CancellationToken cancellationToken)
            {
                var totalCount = await context.Authors.CountAsync(cancellationToken);

                var authors = await context.Authors
                    .OrderBy(a => a.Name)
                    .Skip((query.PageNumber - 1) * query.PageSize)
                    .Take(query.PageSize)
                    .Select(a => new AuthorResponse(
                        a.Id.ToString(),
                        a.Name,
                        a.Biography,
                        a.BirthDate,
                        a.DeathDate))
                    .ToListAsync(cancellationToken);

                return new Response(totalCount, authors);
            }
        }
    }
}
```

### 2. Add Tests for the GetList Handler

#### Using Command Line/VS Code

1. Create a new file named `AuthorQueryTests.cs` in the `Library.Tests` project:

    ```bash
    touch tests/Library.Tests/Catalog/Application/Author/AuthorQueryTests.cs
    ```

#### Using Visual Studio

1. Right-click on the **Author** folder in **Solution Explorer** under the `Library.Tests` project.
2. Select **Add** > **Class**.
3. Name the class `AuthorQueryTests` and click **Add**.

#### Add Test Code to Verify Author Creation and Query

Open the `AuthorQueryTests.cs` file and add the following code:

```csharp
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

            await using var context = new CatalogDbContext(options);

            var createHandler = new Create.Handler(context);
            var sut = new GetList.Handler(context);

            var author1 = new Create.Command("Author One", "Biography One", DateTimeOffset.Now.AddYears(-40), null);
            var author2 = new Create.Command("Author Two", "Biography Two", DateTimeOffset.Now.AddYears(-30), null);

            await createHandler.Handle(author1, CancellationToken.None);
            await createHandler.Handle(author2, CancellationToken.None);

            var query = new GetList.Query(1, 10);

            // Act
            var result = await sut.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.TotalCount, Is.EqualTo(2));
            Assert.That(result.Authors.Count, Is.EqualTo(2));
            Assert.That(result.Authors[0].Name, Is.EqualTo("Author One"));
            Assert.That(result.Authors[1].Name, Is.EqualTo("Author Two"));
        }
    }
}
```

### Conclusion

You have successfully created a MediatR handler to query for authors with paging support and added a test that verifies the functionality. This lab demonstrated how to implement and test querying capabilities in your application using MediatR and EF Core.