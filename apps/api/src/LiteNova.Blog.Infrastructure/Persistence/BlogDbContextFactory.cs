using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LiteNova.Blog.Infrastructure.Persistence;

internal sealed class BlogDbContextFactory : IDesignTimeDbContextFactory<BlogDbContext>
{
    public BlogDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__Database")
            ?? "Host=localhost;Port=5433;Database=blog;Username=blog;Password=blog";

        var optionsBuilder = new DbContextOptionsBuilder<BlogDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new BlogDbContext(optionsBuilder.Options, new NoOpEventMediator());
    }
}
