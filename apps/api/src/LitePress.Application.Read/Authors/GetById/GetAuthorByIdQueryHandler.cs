using LitePress.Application.Read.Contracts.Authors.GetAuthorById;
using LitePress.Domain.Authors.Exceptions;

namespace LitePress.Application.Read.Authors.GetById;

internal sealed class GetAuthorByIdQueryHandler : IQueryHandler<GetAuthorByIdQuery, AuthorResult>
{
    private readonly IReadDatabase _db;
    public GetAuthorByIdQueryHandler(IReadDatabase db) { _db = db; }

    public Task<AuthorResult> HandleAsync(GetAuthorByIdQuery query, CancellationToken cancellationToken) =>
        _db.QueryAsync(async (ctx, ct) =>
        {
            var author = await ctx.LoadAsync<Author>(query.AuthorId, ct)
                ?? throw new AuthorNotFoundException(query.AuthorId);

            return new AuthorResult(author.Id.Value, author.DisplayName);
        }, cancellationToken);
}
