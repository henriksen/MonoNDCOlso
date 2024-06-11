using Library.Catalog.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Library.Catalog.Application.Author
{
    public class GetList
    {
        public record Query(int PageNumber, int PageSize) : IRequest<Response>;

        public record Response(int TotalCount, List<AuthorResponse> Authors);

        public record AuthorResponse(string Id, string Name, DateTimeOffset? BirthDate, DateTimeOffset? DeathDate);

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
                        a.BirthDate,
                        a.DeathDate))
                    .ToListAsync(cancellationToken);

                return new Response(totalCount, authors);
            }
        }
    }
}