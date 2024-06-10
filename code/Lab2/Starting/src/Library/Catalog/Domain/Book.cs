using Library.Catalog.Contracts;

namespace Library.Catalog.Domain
{
    public class Book
    {
        public Book(BookId id,
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

        public BookId Id { get; }
        public string Title { get; private set; }
        public AuthorId AuthorId { get; private set; }
        public string ISBN { get; private set; }
        public PublisherId PublisherId { get; private set; }
        public DateTimeOffset PublicationDate { get; private set; }
        public GenreId GenreId { get; private set; }
        public string Description { get; private set; }

        public void UpdateTitle(string title)
        {
            Title = title;
        }

        public void UpdateAuthor(AuthorId authorId)
        {
            AuthorId = authorId;
        }

        public void UpdateISBN(string isbn)
        {
            ISBN = isbn;
        }

        public void UpdatePublisher(PublisherId publisherId)
        {
            PublisherId = publisherId;
        }

        public void UpdatePublicationDate(DateTimeOffset publicationDate)
        {
            PublicationDate = publicationDate;
        }

        public void UpdateGenre(GenreId genreId)
        {
            GenreId = genreId;
        }

        public void UpdateDescription(string description)
        {
            Description = description;
        }
    }

}
