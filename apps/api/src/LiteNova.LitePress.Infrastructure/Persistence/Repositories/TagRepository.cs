using LiteNova.LitePress.Domain.Tags.Exceptions;
using LiteNova.LitePress.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LiteNova.LitePress.Infrastructure.Persistence.Repositories;

internal sealed class TagRepository : ITagRepository
{
    private readonly LitePressDbContext _context;

    public TagRepository(LitePressDbContext context)
    {
        _context = context;
    }

    public async Task<Tag> GetByIdAsync(TagId id, CancellationToken cancellationToken = default)
    {
        return await _context.Tags.FirstOrDefaultAsync(t => t.Id == id, cancellationToken)
            ?? throw new TagNotFoundException(id);
    }

    public async Task<bool> NameExistsAsync(TagName name, CancellationToken cancellationToken = default)
    {
        return await _context.Tags.AnyAsync(t => t.Name == name, cancellationToken);
    }

    public async Task AddAsync(Tag tag, CancellationToken cancellationToken = default)
    {
        await _context.Tags.AddAsync(tag, cancellationToken);
    }

    public Task UpdateAsync(Tag tag, CancellationToken cancellationToken = default)
    {
        _context.Tags.Update(tag);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Tag tag, CancellationToken cancellationToken = default)
    {
        _context.Tags.Remove(tag);
        return Task.CompletedTask;
    }
}
