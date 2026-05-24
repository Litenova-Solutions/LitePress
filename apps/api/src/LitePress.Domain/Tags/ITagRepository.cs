namespace LitePress.Domain.Tags;

public interface ITagRepository
{
    public Task<Tag> GetByIdAsync(TagId id, CancellationToken cancellationToken);
    public Task<bool> NameExistsAsync(TagName name, CancellationToken cancellationToken);
    public Task AddAsync(Tag tag, CancellationToken cancellationToken);
    public Task UpdateAsync(Tag tag, CancellationToken cancellationToken);
    public Task DeleteAsync(Tag tag, CancellationToken cancellationToken);
}
