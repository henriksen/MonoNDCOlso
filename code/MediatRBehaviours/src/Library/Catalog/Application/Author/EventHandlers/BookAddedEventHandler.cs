using Library.Catalog.Contracts;
using Library.Catalog.Domain.Events;
using Library.Catalog.Infrastructure.Data;
using MediatR;

namespace Library.Catalog.Application.Author.EventHandlers
{
    public class UpdateAuthorBooksOnBookAddedEvent(CatalogDbContext context) : INotificationHandler<BookAdded>
    {
        public Task Handle(BookAdded notification, CancellationToken cancellationToken)
        {
            var author = context.Authors.Find(new AuthorId(notification.AuthorId));
            if (author == null)
            {
                throw new InvalidOperationException($"Author with id {notification.AuthorId} was not found");
            }
            author.AddBook(notification.BookId);
            context.SaveChanges();
            return Task.CompletedTask;
        }
    }
}
