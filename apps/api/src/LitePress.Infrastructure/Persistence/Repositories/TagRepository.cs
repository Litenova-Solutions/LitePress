using LitePress.Domain.Tags.Exceptions;
using LitePress.Infrastructure.Marten;

namespace LitePress.Infrastructure.Persistence.Repositories;

internal sealed class TagRepository(IMartenUnitOfWork unitOfWork) : ITagRepository
{
    public async Task<Tag> GetByIdAsync(TagId id, CancellationToken cancellationToken = default)
    {
        var tag = await unitOfWork.Session.LoadAsync<Tag>(id, cancellationToken);
        return tag ?? throw new TagNotFoundException(id);
    }

    public async Task<bool> NameExistsAsync(TagName name, CancellationToken cancellationToken = default) =>
        await unitOfWork.Session.Query<Tag>()
            .AnyAsync(tag => tag.Name.Value == name.Value, cancellationToken);

    public Task AddAsync(Tag tag, CancellationToken cancellationToken = default)
    {
        unitOfWork.StoreAndTrack(tag);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Tag tag, CancellationToken cancellationToken = default)
    {
        unitOfWork.StoreAndTrack(tag);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Tag tag, CancellationToken cancellationToken = default)
    {
        unitOfWork.DeleteAndTrack(tag);
        return Task.CompletedTask;
    }
}
