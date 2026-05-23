using LiteNova.LitePress.Domain.Authors;
using LiteNova.LitePress.Domain.Authors.Exceptions;
using LiteNova.LitePress.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LiteNova.LitePress.Infrastructure.Persistence.Repositories;

internal sealed class AuthorRepository : IAuthorRepository
{
    private readonly LitePressDbContext _context;

    public AuthorRepository(LitePressDbContext context)
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
