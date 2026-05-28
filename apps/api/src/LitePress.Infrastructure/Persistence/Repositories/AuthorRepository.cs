using LitePress.Domain.Authors.Exceptions;
using LitePress.Infrastructure.Marten;

namespace LitePress.Infrastructure.Persistence.Repositories;

internal sealed class AuthorRepository(IMartenUnitOfWork unitOfWork) : IAuthorRepository
{
    public async Task<Author> GetByIdAsync(AuthorId id, CancellationToken cancellationToken = default)
    {
        var author = await unitOfWork.Session.LoadAsync<Author>(id, cancellationToken);
        return author ?? throw new AuthorNotFoundException(id);
    }

    public async Task<Author?> FindByExternalIdAsync(string externalId, CancellationToken cancellationToken = default) =>
        await unitOfWork.Session.Query<Author>()
            .FirstOrDefaultAsync(author => author.ExternalId == externalId, cancellationToken);

    public Task AddAsync(Author author, CancellationToken cancellationToken = default)
    {
        unitOfWork.StoreAndTrack(author);
        return Task.CompletedTask;
    }
}
