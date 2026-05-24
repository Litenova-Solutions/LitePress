using LitePress.Application.Read.Contracts.Shared;
using LitePress.Application.Reactions.Posts.OnPostCreated;
using LitePress.Application.Write.Posts.Create;
using LitePress.Application.Read.Posts.GetById;
using LitePress.Domain.Authors;
using LitePress.Domain.Posts;
using LitePress.Domain.Tags;
using LitePress.Infrastructure.Behaviors;
using LitePress.Infrastructure.Persistence;
using LitePress.Infrastructure.Persistence.Repositories;
using EFCore.NamingConventions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LitePress.Infrastructure.DependencyInjection;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<LitePressDbContext>(options =>
            options
                .UseNpgsql(configuration.GetConnectionString("Database"))
                .UseSnakeCaseNamingConvention());

        services.AddScoped<IDatabaseContext>(
            sp => sp.GetRequiredService<LitePressDbContext>());

        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<ITagRepository, TagRepository>();
        services.AddScoped<IAuthorRepository, AuthorRepository>();

        return services;
    }
}
