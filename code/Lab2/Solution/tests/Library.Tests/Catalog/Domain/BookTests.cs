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