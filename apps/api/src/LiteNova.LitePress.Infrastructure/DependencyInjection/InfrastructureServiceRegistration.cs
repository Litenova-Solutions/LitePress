using LiteNova.LitePress.Application.Read.Contracts.Shared;
using LiteNova.LitePress.Application.Reactions.Posts.OnPostCreated;
using LiteNova.LitePress.Application.Write.Posts.Create;
using LiteNova.LitePress.Application.Read.Posts.GetById;
using LiteNova.LitePress.Domain.Authors;
using LiteNova.LitePress.Domain.Posts;
using LiteNova.LitePress.Domain.Tags;
using LiteNova.LitePress.Infrastructure.Behaviors;
using LiteNova.LitePress.Infrastructure.Persistence;
using LiteNova.LitePress.Infrastructure.Persistence.Repositories;
using EFCore.NamingConventions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LiteNova.LitePress.Infrastructure.DependencyInjection;

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
