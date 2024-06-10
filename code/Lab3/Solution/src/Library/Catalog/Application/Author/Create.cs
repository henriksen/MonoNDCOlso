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
            var author = Domain.Author.Create(AuthorId.New(), command.Name, command.Biography, command.BirthDate, command.DeathDate);
            context.Authors.Add(author);
            await context.SaveChangesAsync(cancellationToken);
            return author.Id;
        }
    }
}