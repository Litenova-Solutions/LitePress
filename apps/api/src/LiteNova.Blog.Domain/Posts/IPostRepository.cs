namespace LiteNova.Blog.Domain.Posts;

public interface IPostRepository
{
    public Task<Post> GetByIdAsync(PostId id, CancellationToken cancellationToken);
    public Task<bool> SlugExistsAsync(PostSlug slug, CancellationToken cancellationToken);
    public Task AddAsync(Post post, CancellationToken cancellationToken);
    public Task UpdateAsync(Post post, CancellationToken cancellationToken);
    public Task DeleteAsync(Post post, CancellationToken cancellationToken);
}
