using LitePress.Domain.Posts.Exceptions;
using LitePress.Infrastructure.Marten;

namespace LitePress.Infrastructure.Persistence.Repositories;

internal sealed class PostRepository(IMartenUnitOfWork unitOfWork) : IPostRepository
{
    public async Task<Post> GetByIdAsync(PostId id, CancellationToken cancellationToken = default)
    {
        var post = await unitOfWork.Session.LoadAsync<Post>(id, cancellationToken);
        return post ?? throw new PostNotFoundException(id);
    }

    public async Task<bool> SlugExistsAsync(PostSlug slug, CancellationToken cancellationToken = default) =>
        await unitOfWork.Session.Query<Post>()
            .AnyAsync(post => post.Slug.Value == slug.Value, cancellationToken);

    public Task AddAsync(Post post, CancellationToken cancellationToken = default)
    {
        unitOfWork.StoreAndTrack(post);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Post post, CancellationToken cancellationToken = default)
    {
        unitOfWork.StoreAndTrack(post);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Post post, CancellationToken cancellationToken = default)
    {
        unitOfWork.DeleteAndTrack(post);
        return Task.CompletedTask;
    }
}
