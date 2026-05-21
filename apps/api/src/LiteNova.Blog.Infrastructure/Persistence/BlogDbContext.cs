using LiteBus.Commands.Abstractions;
using LiteBus.Events.Abstractions;
using LiteNova.Blog.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace LiteNova.Blog.Infrastructure.Persistence;

internal sealed class BlogDbContext : DbContext, LiteNova.Blog.Application.Read.Contracts.Shared.IDatabaseContext
{
    private readonly IEventMediator _eventMediator;

    public BlogDbContext(DbContextOptions<BlogDbContext> options, IEventMediator eventMediator)
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
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BlogDbContext).Assembly);

        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            // Owned entity types share the owner's table; their columns are explicitly
            // configured in OwnsOne blocks, so skip them here to avoid shadow-property conflicts.
            if (entity.IsOwned())
            {
                continue;
            }

            var tableName = entity.GetTableName();
            if (!string.IsNullOrEmpty(tableName))
            {
                entity.SetTableName(ToSnakeCase(tableName));
            }

            foreach (var property in entity.GetProperties())
            {
                var columnName = property.GetColumnName();
                if (!string.IsNullOrEmpty(columnName))
                {
                    property.SetColumnName(ToSnakeCase(columnName));
                }
            }
        }
    }

    private static string ToSnakeCase(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return name;
        }

        var result = System.Text.RegularExpressions.Regex.Replace(
            System.Text.RegularExpressions.Regex.Replace(name, @"([A-Z]+)([A-Z][a-z])", "$1_$2"),
            @"([a-z\d])([A-Z])", "$1_$2");

        return result.ToLowerInvariant();
    }
}
