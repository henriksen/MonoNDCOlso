using FluentResults;
using Library.Catalog.Contracts;
using Library.Catalog.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Library.Catalog.Application.Author
{
    public class Get
    {
        public record Query(AuthorId id) : IRequest<Result<AuthorResponse>>;

        public record AuthorNotFoundError() : IError
        {
            public required AuthorId Id { get; init; }
            public int ErrorCode { get; } = 404;
            public string Message { get; } = "Author was not found";

            public Dictionary<string, object> Metadata { get; } = [];
            public List<IError> Reasons { get; } = [];
        }

        public record AuthorResponse(AuthorId Id, string Name, string Biography, DateTimeOffset? BirthDate, DateTimeOffset? DeathDate);

        public class Handler(CatalogDbContext context) : IRequestHandler<Query, Result<AuthorResponse>>
        {
            public async Task<Result<AuthorResponse>> Handle(Query query, CancellationToken cancellationToken)
            {

                var author = await context.Authors
                    .Where(a => a.Id == query.id)
                    .Select(a => new AuthorResponse(
                        a.Id,
                        a.Name,
                        a.Biography,
                        a.BirthDate,
                        a.DeathDate))
                    .FirstOrDefaultAsync(cancellationToken: cancellationToken);

                if (author == null)
                {
                    return Result.Fail(new AuthorNotFoundError() { Id = query.id });
                }

                return Result.Ok(author);
            }
        }
    }
}