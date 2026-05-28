using LitePress.Application.Read.Contracts.Shared;
using LitePress.Application.Write.Contracts.Shared;
using LitePress.Infrastructure.Marten;
using LitePress.Infrastructure.Persistence.Repositories;
using LitePress.Infrastructure.Time;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LitePress.Infrastructure.DependencyInjection;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<IClock, SystemClock>();
        services.AddMartenStore(configuration);
        services.AddScoped<IReadDatabase, MartenReadDatabase>();
        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<ITagRepository, TagRepository>();
        services.AddScoped<IAuthorRepository, AuthorRepository>();

        return services;
    }
}
