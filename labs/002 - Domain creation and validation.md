# Domain object creation and validation

Let's rewrite the `Book` class using a private constructor and a static `Book.Create(...)` method. This pattern is often used to enforce certain invariants or business rules at the time of object creation.

In the `Catalog/Domain/Book.cs`, make the constructor private and add a static `Create` method to create new instances of the `Book` class.

```csharp
        [... other code exluded for brevity ...]

        private Book(
            BookId id,
            string title,
            AuthorId authorId,
            string isbn,
            PublisherId publisherId,
            DateTimeOffset publicationDate,
            GenreId genreId,
            string description)
        {
            Id = id;
            Title = title;
            AuthorId = authorId;
            ISBN = isbn;
            PublisherId = publisherId;
            PublicationDate = publicationDate;
            GenreId = genreId;
            Description = description;
        }

        public static Book Create(
            BookId id,
            string title,
            AuthorId authorId,
            string isbn,
            PublisherId publisherId,
            DateTimeOffset publicationDate,
            GenreId genreId,
            string description)
        {
            // Perform any validation or business rules enforcement here
            if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Title cannot be empty.", nameof(title));
            if (string.IsNullOrWhiteSpace(isbn)) throw new ArgumentException("ISBN cannot be empty.", nameof(isbn));
            if (publicationDate == default) throw new ArgumentException("Publication date is required.", nameof(publicationDate));

            return new Book(id, title, authorId, isbn, publisherId, publicationDate, genreId, description);
        }

        [... other code exluded for brevity ...]

```

## Adding an NUnit Test Project for "Library"

In this section, you will create an NUnit test project named "Library.Tests" in the `/tests` directory. You will then delete the initial test file, create the necessary folder structure, and add a new test file named `BookTests` to test the validation rules in the `Create` method.

## Steps

### 1. Create the Test Project

#### Using Command Line/VS Code

1. Create a `tests` directory if it doesn't exist:

    ```bash
    mkdir tests
    ```

2. Run the following command to create an NUnit test project named `Library.Tests` inside the `tests` directory:

    ```bash
    dotnet new nunit -n Library.Tests -o tests/Library.Tests
    ```

3. Add the `Library.Tests` project to the solution:

    ```bash
    dotnet sln Library.sln add tests/Library.Tests/Library.Tests.csproj
    ```

#### Using Visual Studio

1. Create a `tests` directory in your solution folder.
2. Right-click on the **Library** solution in **Solution Explorer**.
3. Select **Add** > **New Project**.
4. In the **Add a new project** dialog, select **NUnit Test Project**.
5. Click **Next**.
6. Name the project `Library.Tests`.
7. Set the location to the `tests` directory inside your solution folder.
8. Click **Create**.
9. Select **.NET 8.0 (Standard Term Support)** and click **Create**.

#### Add a Reference to the Library Project from the Tests Project

##### Using Command Line/VS Code

1. Run the following command to add a reference to the `Library` project from the `Library.Tests` project:

    ```bash
    dotnet add tests/Library.Tests/Library.Tests.csproj reference src/Library/Library.csproj
    ```

##### Using Visual Studio

1. Right-click on the **Library.Tests** project in **Solution Explorer**.
2. Select **Add** > **Project Reference**.
3. In the **Reference Manager** dialog, check the box next to the **Library** project.
4. Click **OK**.

### 2. Delete the Initial Test File

#### Using Command Line/VS Code

1. Delete the initial `UnitTest1.cs` file:

    ```bash
    rm tests/Library.Tests/UnitTest1.cs
    ```

#### Using Visual Studio

1. Expand the **Library.Tests** project in **Solution Explorer**.
2. Right-click on the `UnitTest1.cs` file and select **Delete**.

### 3. Create the Folder Structure and Test File

#### Using Command Line/VS Code

1. Create the necessary folders:

    ```bash
    mkdir -p tests/Library.Tests/Catalog/Domain
    ```

2. Create a new test file named `BookTests.cs` in the `Catalog/Domain` folder:

    ```bash
    touch tests/Library.Tests/Catalog/Domain/BookTests.cs
    ```

#### Using Visual Studio

1. Right-click on the **Library.Tests** project in **Solution Explorer**.
2. Select **Add** > **New Folder** and name it `Catalog`.
3. Right-click on the **Catalog** folder and select **Add** > **New Folder** and name it `Domain`.
4. Right-click on the **Domain** folder and select **Add** > **Class**.
5. Name the class `BookTests` and click **Add**.

### 4. Add Tests for the `Create` Method

Open the `BookTests.cs` file and add the following code to test the three validation rules in the `Create` method:

```csharp
using Library.Catalog.Domain;
using Library.Catalog.Contracts;

namespace Library.Tests.Catalog.Domain
{
    public class BookTests
    {
        [Test]
        public void Create_ShouldThrowException_WhenTitleIsEmpty()
        {
            // Arrange
            var bookId = BookId.New();
            var authorId = AuthorId.New();
            var publisherId = PublisherId.New();
            var ISBN = "1234567890123";
            var publicationDate = DateTime.UtcNow;
            var genreId = GenreId.New();
            var description = "Book description";

            string title = "";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => Book.Create(bookId, title, authorId, ISBN, publisherId, publicationDate, genreId, description), "Title cannot not be empty.");
        }

        [Test]
        public void Create_ShouldThrowException_WhenISBNIsEmpty()
        {
            // Arrange
            var bookId = BookId.New();
            string title = "Book title";
            var authorId = AuthorId.New();
            var publisherId = PublisherId.New();
            var publicationDate = DateTime.UtcNow;
            var genreId = GenreId.New();
            var description = "Book description";

            var ISBN = "";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => Book.Create(bookId, title, authorId, ISBN, publisherId, publicationDate, genreId, description), "ISBN cannot be empty.");
        }

        [Test]
        public void Create_ShouldThrowException_WhenPublicationDateIsNotSet()
        {
            // Arrange
            var bookId = BookId.New();
            string title = "Book title";
            var authorId = AuthorId.New();
            var publisherId = PublisherId.New();
            var ISBN = "1234567890123";
            var genreId = GenreId.New();
            var description = "Book description";

            DateTimeOffset publicationDate = default;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => Book.Create(bookId, title, authorId, ISBN, publisherId, publicationDate, genreId, description), "Publication date is required.");

        }
    }
}
```

### Repeat for Author

Do the same exercise for the Author class. For simplicity and time, there is no need to add tests for the Author class in this lab.
