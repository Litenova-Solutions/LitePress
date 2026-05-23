using LiteNova.LitePress.Domain.Posts.Exceptions;
using LiteNova.LitePress.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LiteNova.LitePress.Infrastructure.Persistence.Repositories;

internal sealed class PostRepository : IPostRepository
{
    private readonly LitePressDbContext _context;

    public PostRepository(LitePressDbContext context)
    {
        _context = context;
    }

    public async Task<Post> GetByIdAsync(PostId id, CancellationToken cancellationToken = default)
    {
        return await _context.Posts
            .Include("_tags")
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken)
            ?? throw new PostNotFoundException(id);
    }

    public async Task<bool> SlugExistsAsync(PostSlug slug, CancellationToken cancellationToken = default)
    {
        return await _context.Posts
            .AnyAsync(p => p.Slug == slug, cancellationToken);
    }

    public async Task AddAsync(Post post, CancellationToken cancellationToken = default)
    {
        await _context.Posts.AddAsync(post, cancellationToken);
    }

    public Task UpdateAsync(Post post, CancellationToken cancellationToken = default)
    {
        _context.Posts.Update(post);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Post post, CancellationToken cancellationToken = default)
    {
        _context.Posts.Remove(post);
        return Task.CompletedTask;
    }
}
