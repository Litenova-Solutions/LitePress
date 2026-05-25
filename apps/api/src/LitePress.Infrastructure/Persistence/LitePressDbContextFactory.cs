using EFCore.NamingConventions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LitePress.Infrastructure.Persistence;

internal sealed class LitePressDbContextFactory : IDesignTimeDbContextFactory<LitePressDbContext>
{
    public LitePressDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__Database")
            ?? "Host=localhost;Port=5433;Database=litepress;Username=litepress;Password=litepress";

        var optionsBuilder = new DbContextOptionsBuilder<LitePressDbContext>();
        optionsBuilder
            .UseNpgsql(connectionString)
            .UseSnakeCaseNamingConvention();

        return new LitePressDbContext(optionsBuilder.Options, new NoOpEventMediator());
    }
}
