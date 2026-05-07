using LiteNova.Blog.Application.Common.Interfaces;
using LiteNova.Blog.Domain.Common;
using LiteNova.Blog.Domain.Posts;
using LiteNova.Blog.Domain.Tags;
using Microsoft.EntityFrameworkCore;

namespace LiteNova.Blog.Infrastructure.Persistence;

public sealed class BlogDbContext(DbContextOptions<BlogDbContext> options) : DbContext(options), IBlogDbContext
{
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Tag> Tags => Set<Tag>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BlogDbContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTimeOffset.UtcNow;
        foreach (var entry in ChangeTracker.Entries<Post>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.GetType().GetProperty("CreatedAt")?.SetValue(entry.Entity, now);
            }
            if (entry.State is EntityState.Added or EntityState.Modified)
            {
                entry.Entity.GetType().GetProperty("UpdatedAt")?.SetValue(entry.Entity, now);
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
