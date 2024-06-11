using Library.Catalog.Contracts;
using MediatR;

namespace Library.Catalog.Domain.Events;

public class BookAdded(BookId bookId, AuthorId authorId) : INotification
{
    public Guid AuthorId { get; } = authorId.Value;
    public Guid BookId { get; } = bookId.Value;
}