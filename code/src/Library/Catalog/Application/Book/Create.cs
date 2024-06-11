using Library.Catalog.Contracts;
using Library.Catalog.Infrastructure.Data;
using MediatR;

namespace Library.Catalog.Application.Book;

public class Create
{
    public record Command(string Title, string ISBN, AuthorId AuthorId, PublisherId PublisherId, DateTimeOffset PublicationDate, GenreId GenreId, string Description) : IRequest<BookId>;

    public class Handler(CatalogDbContext context) : IRequestHandler<Command, BookId>
    {
        public async Task<BookId> Handle(Command command, CancellationToken cancellationToken)
        {

            var book = Domain.Book.Create(BookId.New(), command.Title, command.AuthorId, command.ISBN, command.PublisherId, command.PublicationDate, command.GenreId, command.Description);
            context.Books.Add(book);

            await context.SaveChangesAsync(cancellationToken);

            return book.Id;
        }
    }
}