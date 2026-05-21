namespace LiteNova.Blog.Domain.Authors;

public interface IAuthorRepository
{
    public Task<Author> GetByIdAsync(AuthorId id, CancellationToken cancellationToken);
    public Task<Author?> FindByExternalIdAsync(string externalId, CancellationToken cancellationToken);
    public Task AddAsync(Author author, CancellationToken cancellationToken);
}
