# Lab: Adding an Application Layer and Persistence to the Library Project

In this lab, you will add an application layer to the `Library` project using MediatR, set up EF Core for persistence, and create tests to verify functionality. You will create a command to add an author and use EF Core to save the author to the database.

## Prerequisites

- .NET 8 SDK installed on your machine
- Visual Studio 2022 or Visual Studio Code installed

## Steps

### 1. Add MediatR to the Library Project

#### Using Command Line/VS Code

1. Run the following command to add the MediatR package to the `Library` project:

    ```bash
    dotnet add src/Library/Library.csproj package MediatR
    ```

#### Using Visual Studio

1. Right-click on the **Library** project in **Solution Explorer**.
2. Select **Manage NuGet Packages**.
3. Search for **MediatR**.
4. Click **Install**.

### 2. Add Application Folder Structure

#### Using Command Line/VS Code

1. Create the necessary folders:

    ```bash
    mkdir -p src/Library/Catalog/Application/Author
    ```

2. Create a new file named `Create.cs` in the `Author` folder:

    ```bash
    touch src/Library/Catalog/Application/Author/Create.cs
    ```

#### Using Visual Studio

1. Right-click on the **Catalog** folder in **Solution Explorer**.
2. Select **Add** > **New Folder** and name it `Application`.
3. Right-click on the **Application** folder and select **Add** > **New Folder** and name it `Author`.
4. Right-click on the **Author** folder and select **Add** > **Class**.
5. Name the class `Create` and click **Add**.

### 3. Add the Create Command

Open the `Create.cs` file and add the following code:

```csharp
using Library.Catalog.Contracts;
using MediatR;

namespace Library.Catalog.Application.Author;

public class Create
{
    public record Command(string Name, string Biography, DateTimeOffset? BirthDate, DateTimeOffset? DeathDate) : IRequest<AuthorId>;

    public class Handler : IRequestHandler<Command, AuthorId>
    {
        public async Task<AuthorId> Handle(Command command, CancellationToken cancellationToken)
        {
            var author = new Domain.Author(AuthorId.New(), command.Name, command.Biography, command.BirthDate, command.DeathDate);
            return author.Id;
        }
    }
}
```

### 4. Add EF Core to the Project

#### Using Command Line/VS Code

1. Run the following command to add EF Core packages to the `Library` project:

    ```bash
    dotnet add src/Library/Library.csproj package Microsoft.EntityFrameworkCore.SqlServer
    dotnet add src/Library/Library.csproj package Microsoft.EntityFrameworkCore.Design
    ```

2. Run the following command to add the EF Core In-Memory package to the `Library.Tests` project:

    ```bash
    dotnet add tests/Library.Tests/Library.Tests.csproj package Microsoft.EntityFrameworkCore.InMemory
    ```

#### Using Visual Studio

1. Right-click on the **Library** project in **Solution Explorer**.
2. Select **Manage NuGet Packages**.
3. Search for **Microsoft.EntityFrameworkCore.SqlServer**.
4. Click **Install**.
5. Search for **Microsoft.EntityFrameworkCore.Design**.
6. Click **Install**.

7. Right-click on the **Library.Tests** project in **Solution Explorer**.
8. Select **Manage NuGet Packages**.
9. Search for **Microsoft.EntityFrameworkCore.InMemory**.
10. Click **Install**.

### 5. Add Infrastructure Folder Structure

#### Using Command Line/VS Code

1. Create the necessary folders:

    ```bash
    mkdir -p src/Library/Catalog/Infrastructure/Data
    ```

2. Create a new file named `CatalogDbContext.cs` in the `Data` folder:

    ```bash
    touch src/Library/Catalog/Infrastructure/Data/CatalogDbContext.cs
    ```

#### Using Visual Studio

1. Right-click on the **Catalog** folder in **Solution Explorer**.
2. Select **Add** > **New Folder** and name it `Infrastructure`.
3. Right-click on the **Infrastructure** folder and select **Add** > **New Folder** and name it `Data`.
4. Right-click on the **Data** folder and select **Add** > **Class**.
5. Name the class `CatalogDbContext` and click **Add**.

### 6. Add CatalogDbContext Code

Open the `CatalogDbContext.cs` file and add the following code:

```csharp
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
                entity.Property(e => e.Name).IsRequired();
            });
        }

        public DbSet<Domain.Author> Authors { get; set; }
    }
}
```

### 7. Update the Create Handler

Open the `Create.cs` file in `Catalog/Application/Author` and update the handler to take `CatalogDbContext` as a constructor parameter and save the author to the database:

```csharp
using Library.Catalog.Contracts;
using Library.Catalog.Infrastructure.Data;
using MediatR;

namespace Library.Catalog.Application.Author;

public class Create
{
    public record Command(string Name, string Biography, DateTimeOffset? BirthDate, DateTimeOffset? DeathDate) : IRequest<AuthorId>;

    public class Handler(CatalogDbContext context) : IRequestHandler<Command, AuthorId>
    {
        public async Task<AuthorId> Handle(Command command, CancellationToken cancellationToken)
        {
            var author = new Domain.Author(AuthorId.New(), command.Name, command.Biography, command.BirthDate, command.DeathDate);
            context.Authors.Add(author);
            await context.SaveChangesAsync(cancellationToken);
            return author.Id;
        }
    }
}
```

### 8. Adding an EF Core Design Time Factory

#### Using Command Line/VS Code

Create a new file named `DesignTimeCatalogDbContextFactory.cs` in the `Data` folder:

```bash
touch src/Library/Catalog/Infrastructure/Data/DesignTimeCatalogDbContextFactory.cs
```

#### Using Visual Studio

1. Right-click on the **Data** folder and select **Add** > **Class**.
2. Name the class `DesignTimeCatalogDbContextFactory` and click **Add**.

#### Update the DesignTimeCatalogDbContextFactory Code

Open the `DesignTimeCatalogDbContextFactory.cs` file and add the following code:

```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;

namespace Library.Catalog.Infrastructure.Data
{
    public class DesignTimeCatalogDbContextFactory : IDesignTimeDbContextFactory<CatalogDbContext>
    {
        public CatalogDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CatalogDbContext>();
            optionsBuilder.UseSqlServer("Server=localhost;Database=Library;User Id=sa;Password=Password_123#;");

            return new CatalogDbContext(optionsBuilder.Options);
        }
    }
}
```

> **Note:** Replace the connection string with your actual connection string.

### 9. Configure StronglyTypedIds to use EF Core

### Adding StronglyTypedId Settings

In this step, you will add settings for StronglyTypedIds to the `Library` project by creating a folder at the root and adding the necessary files.

#### Using Command Line/VS Code

1. Create the necessary folder:

    ```bash
    mkdir -p src/Library/SharedKernel/Types
    ```

2. Create a new file named `TypeDefaults.cs` in the `Types` folder:

    ```bash
    touch src/Library/SharedKernel/Types/TypeDefaults.cs
    ```

3. Create a new file named `guid-efcore.typedid` in the `Types` folder:

    ```bash
    touch src/Library/SharedKernel/Types/guid-efcore.typedid
    ```

4. Add the following to the `Library.csproj` file:

    ```xml
    <ItemGroup>
        <AdditionalFiles Include="SharedKernel/Types/guid-efcore.typedid" />
    </ItemGroup>
    ```

#### Using Visual Studio

1. Right-click on the **Library** project in **Solution Explorer**.
2. Select **Add** > **New Folder** and name it `SharedKernel`.
3. Right-click on the **SharedKernel** folder and select **Add** > **New Folder** and name it `Types`.
4. Right-click on the **Types** folder and select **Add** > **Class**.
5. Name the class `TypeDefaults` and click **Add**.
6. Right-click on the **Types** folder and select **Add** > **New Item**.
7. Name the file `guid-efcore.typedid` and click **Add**.
8. Right-click the `guid-efcore.typedid` file, select **Properties**, and set the **Build Action** to `C# analyzer additional file`.

#### Add the file content

1. Open the `TypeDefaults.cs` file and add the following code:

    ```csharp
    using StronglyTypedIds;

    [assembly: StronglyTypedIdDefaults(Template.Guid, "guid-efcore")]
    ```

2. Open the `guid-efcore.typedid` file and add the following code:

    ```csharp
    partial struct PLACEHOLDERID
    {
        public partial class EfCoreValueConverter : global::Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter<PLACEHOLDERID, global::System.Guid>
        {
            public EfCoreValueConverter() : this(null) { }
            public EfCoreValueConverter(global::Microsoft.EntityFrameworkCore.Storage.ValueConversion.ConverterMappingHints? mappingHints = null)
                : base(
                    id => id.Value,
                    value => new PLACEHOLDERID(value),
                    mappingHints
                ) { }
        }
    }
    ```

### 10. Adding a Private Constructor to the Author Class for EF Core

Entity Framework Core (EF Core) requires an entity class to have a parameterless constructor so that it can create instances of the entity when materializing results from a query. This constructor can be private to ensure that it is only used by EF Core and not by other parts of your application.

1. **Locate the `Author` Class:**
   Find the `Author` class in your in the `Domain` folder within the `Catalog` module.

2. **Add a Private Parameterless Constructor:**
   Modify the `Author` class to include a private parameterless constructor. This ensures that EF Core can create instances of the class, but other parts of the application cannot.

```csharp
public class Author
{
    [...]

    // Private parameterless constructor for EF Core
    private Author() { }

    [...]
}
```

### 11. Run Migrations

#### Using Command Line/VS Code

1. Add the necessary tools for EF Core migrations:

    ```bash
    dotnet tool install --global dotnet-ef
    ```

2. Add a folder for the migrations:

    ```bash
    mkdir -p src/Library/Catalog/Infrastructure/Data/Migrations
    ```

3. Run the following commands to add and apply migrations:

    ```bash
    dotnet ef migrations add InitialCreate -o Catalog/Infrastructure/Data/Migrations --project ./src/Library
    dotnet ef database update --project ./src/Library
    ```

#### Using Visual Studio

1. Open the **Package Manager Console** from **Tools** > **NuGet Package Manager** > **Package Manager Console**.
2. Set the default project to `Library`.
3. Run the following command to add and apply migrations:

    ```powershell
    Add-Migration InitialCreate -OutputDir Catalog/Infrastructure/Data/Migrations
    Update-Database
    ```

### 9. Add Tests for the Create Command

#### Using Command Line/VS Code

1. Create a new file named `AuthorTests.cs` in the `Library.Tests` project:

    ```bash
    mkdir -p tests/Library.Tests/Catalog/Application/Author
    touch tests/Library.Tests/Catalog/Application/Author/AuthorTests.cs
    ```

#### Using Visual Studio

1. Right-click on the **Library.Tests** project in **Solution Explorer**.
2. Select **Add** > **New Folder** and name it `Catalog`.
3. Right-click on the **Catalog** folder and select **Add** > **New Folder** and name it `Application`.
4. Right-click on the **Application** folder and select **Add** > **New Folder** and name it `Author`.
5. Right-click on the **Author** folder and select **Add** > **Class**.
6. Name the class `AuthorTests` and click **Add**.

### 10. Add Test Code to Verify Author Creation

Open the `AuthorTests.cs` file and add the following code:

```csharp
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
```

### Conclusion

You have successfully added an application layer using MediatR, set up EF Core for persistence,

You have also created tests to verify that an author is saved to the database.
