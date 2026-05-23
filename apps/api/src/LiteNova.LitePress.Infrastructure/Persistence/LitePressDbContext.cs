using LiteBus.Commands.Abstractions;
using LiteBus.Events.Abstractions;
using LiteNova.LitePress.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace LiteNova.LitePress.Infrastructure.Persistence;

internal sealed class LitePressDbContext : DbContext, LiteNova.LitePress.Application.Read.Contracts.Shared.IDatabaseContext
{
    private readonly IEventMediator _eventMediator;

    public LitePressDbContext(DbContextOptions<LitePressDbContext> options, IEventMediator eventMediator)
        : base(options)
    {
        _eventMediator = eventMediator;
    }

    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<Author> Authors => Set<Author>();

    IQueryable<Post> Application.Read.Contracts.Shared.IDatabaseContext.Posts => Posts;
    IQueryable<Tag> Application.Read.Contracts.Shared.IDatabaseContext.Tags => Tags;
    IQueryable<Author> Application.Read.Contracts.Shared.IDatabaseContext.Authors => Authors;

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var aggregates = ChangeTracker.Entries<IAggregateRoot>()
            .Select(e => e.Entity)
            .ToList();

        var domainEvents = aggregates
            .SelectMany(a => a.GetDomainEvents())
            .ToList();

        foreach (var aggregate in aggregates)
        {
            aggregate.ClearDomainEvents();
        }

        var result = await base.SaveChangesAsync(cancellationToken);

        foreach (var domainEvent in domainEvents)
        {
            await _eventMediator.PublishAsync(domainEvent, cancellationToken: cancellationToken);
        }

        return result;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LitePressDbContext).Assembly);
    }
}
