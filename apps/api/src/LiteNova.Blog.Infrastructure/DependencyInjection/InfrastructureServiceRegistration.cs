using LiteNova.Blog.Application.Read.Contracts.Shared;
using LiteNova.Blog.Application.Reactions.Posts.OnPostCreated;
using LiteNova.Blog.Application.Write.Posts.Create;
using LiteNova.Blog.Application.Read.Posts.GetById;
using LiteNova.Blog.Domain.Authors;
using LiteNova.Blog.Domain.Posts;
using LiteNova.Blog.Domain.Tags;
using LiteNova.Blog.Infrastructure.Behaviors;
using LiteNova.Blog.Infrastructure.Persistence;
using LiteNova.Blog.Infrastructure.Persistence.Repositories;
using EFCore.NamingConventions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LiteNova.Blog.Infrastructure.DependencyInjection;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<BlogDbContext>(options =>
            options
                .UseNpgsql(configuration.GetConnectionString("Database"))
                .UseSnakeCaseNamingConvention());

        services.AddScoped<IDatabaseContext>(
            sp => sp.GetRequiredService<BlogDbContext>());

        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<ITagRepository, TagRepository>();
        services.AddScoped<IAuthorRepository, AuthorRepository>();

        return services;
    }
}
