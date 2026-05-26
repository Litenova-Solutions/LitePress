using LitePress.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace LitePress.Infrastructure.Persistence;

internal sealed class LitePressDbContext : DbContext, LitePress.Application.Read.Contracts.Shared.IDatabaseContext
{
    public LitePressDbContext(DbContextOptions<LitePressDbContext> options)
        : base(options)
    {
    }

    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<Author> Authors => Set<Author>();

    IQueryable<Post> Application.Read.Contracts.Shared.IDatabaseContext.Posts => Posts;
    IQueryable<Tag> Application.Read.Contracts.Shared.IDatabaseContext.Tags => Tags;
    IQueryable<Author> Application.Read.Contracts.Shared.IDatabaseContext.Authors => Authors;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LitePressDbContext).Assembly);
    }
}
