using LitePress.Application.Read.Contracts.Authors.GetAuthorById;
using LitePress.Domain.Authors.Exceptions;

namespace LitePress.Application.Read.Authors.GetById;

internal sealed class GetAuthorByIdQueryHandler : IQueryHandler<GetAuthorByIdQuery, AuthorResult>
{
    private readonly IDatabaseContext _db;
    public GetAuthorByIdQueryHandler(IDatabaseContext db) { _db = db; }

    public async Task<AuthorResult> HandleAsync(GetAuthorByIdQuery query, CancellationToken cancellationToken)
    {
        var author = await _db.Authors
            .AsNoTracking()
            .Where(a => a.Id == query.AuthorId)
            .Select(a => new AuthorResult(a.Id.Value, a.DisplayName))
            .FirstOrDefaultAsync(cancellationToken);

        if (author is null)
        {
            throw new AuthorNotFoundException(query.AuthorId);
        }

        return author;
    }
}