using LitePress.Domain.Authors;
using LitePress.Domain.Posts;
using LitePress.Domain.Tags;
using Marten;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LitePress.Infrastructure.Marten;

/// <summary>
/// Registers Marten <see cref="IDocumentStore"/>, lightweight sessions, and <see cref="IMartenUnitOfWork"/>.
/// Called from <c>AddInfrastructure</c>. <see cref="ConfigureStore"/> is also used by test fixtures to apply the same document mapping.
/// </summary>
internal static class MartenStoreRegistration
{
    /// <summary>
    /// Adds Marten to DI using <c>ConnectionStrings:DefaultConnection</c> and project store options.
    /// </summary>
    internal static IServiceCollection AddMartenStore(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMarten(options =>
            {
                options.Connection(MartenConnection.GetRequired(configuration));
                ConfigureStore(options);
            })
            .UseLightweightSessions();

        services.AddScoped<IMartenUnitOfWork, MartenUnitOfWork>();

        return services;
    }

    /// <summary>
    /// Shared document store configuration: JSON serialization, and identity mapping for aggregate roots.
    /// </summary>
    internal static void ConfigureStore(StoreOptions options)
    {
        Serialization.Internal.MartenJsonSerializerConfigurer.Configure(options);

        options.Schema.For<Post>().Identity(post => post.Id);
        options.Schema.For<Author>().Identity(author => author.Id);
        options.Schema.For<Tag>().Identity(tag => tag.Id);
    }
}
