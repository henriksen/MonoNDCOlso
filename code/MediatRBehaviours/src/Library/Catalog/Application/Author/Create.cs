using FluentResults;
using FluentValidation;
using Library.Catalog.Contracts;
using Library.Catalog.Infrastructure.Data;
using MediatR;

namespace Library.Catalog.Application.Author;

public class Create
{
    public record Command(string Name, string Biography, DateTimeOffset? BirthDate, DateTimeOffset? DeathDate) : IRequest<Result<AuthorId>>;

    public class Handler(CatalogDbContext context) : IRequestHandler<Command, Result<AuthorId>>
    {
        public async Task<Result<AuthorId>> Handle(Command command, CancellationToken cancellationToken)
        {
            var result = Domain.Author.Create(AuthorId.New(), command.Name, command.Biography, command.BirthDate, command.DeathDate);

            if (result.IsFailed)
            {
                // throw new InvalidOperationException(string.Join(", ", result.Errors)));
                return Result.Fail(result.Errors);
            }

            context.Authors.Add(result.Value);
            await context.SaveChangesAsync(cancellationToken);
            return result.Value.Id;
        }
    }

    public class CommandRequiredFieldsValidator : AbstractValidator<Command>
    {
        public CommandRequiredFieldsValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Biography).NotEmpty();
        }
    }
    public class CommandBirthdateValidator : AbstractValidator<Command>
    {
        public CommandBirthdateValidator()
        {
            When(x => x.BirthDate is not null, () =>
            {
                RuleFor(x => x.BirthDate).LessThan(x => x.DeathDate)
                    .When(c => c.DeathDate is not null);
                RuleFor(x => x.BirthDate < DateTimeOffset.UtcNow);
            });
        }
    }
}