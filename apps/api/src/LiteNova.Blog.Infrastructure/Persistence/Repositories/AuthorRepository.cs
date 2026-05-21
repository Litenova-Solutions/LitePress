using LiteNova.Blog.Domain.Authors;
using LiteNova.Blog.Domain.Authors.Exceptions;
using LiteNova.Blog.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LiteNova.Blog.Infrastructure.Persistence.Repositories;

internal sealed class AuthorRepository : IAuthorRepository
{
    private readonly BlogDbContext _context;

    public AuthorRepository(BlogDbContext context)
    {
        _context = context;
    }

    public async Task<Author> GetByIdAsync(AuthorId id, CancellationToken cancellationToken = default)
    {
        return await _context.Authors.FirstOrDefaultAsync(a => a.Id == id, cancellationToken)
            ?? throw new AuthorNotFoundException(id);
    }

    public async Task<Author?> FindByExternalIdAsync(string externalId, CancellationToken cancellationToken = default)
    {
        return await _context.Authors.FirstOrDefaultAsync(a => a.ExternalId == externalId, cancellationToken);
    }

    public async Task AddAsync(Author author, CancellationToken cancellationToken = default)
    {
        await _context.Authors.AddAsync(author, cancellationToken);
    }
}
