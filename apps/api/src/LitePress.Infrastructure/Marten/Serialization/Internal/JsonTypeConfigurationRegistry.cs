using LitePress.Infrastructure.Marten.Serialization.Abstractions.Configurations;
using LitePress.Infrastructure.Marten.Serialization.Abstractions.Conventions;
using System.Text.Json.Serialization.Metadata;

namespace LitePress.Infrastructure.Marten.Serialization.Internal;

/// <summary>
/// Applies conventions and per-aggregate JSON configurations when System.Text.Json builds <see cref="JsonTypeInfo"/>.
/// </summary>
internal static class JsonTypeConfigurationRegistry
{
    private static readonly IJsonSerializationConvention[] Conventions =
    [
        new Conventions.DomainTypeSerializationConvention(),
    ];

    private static readonly IAggregateRootJsonConfiguration[] AggregateConfigurations =
    [
        new Aggregates.PostAggregateJsonConfiguration(),
        new Aggregates.AuthorAggregateJsonConfiguration(),
        new Aggregates.TagAggregateJsonConfiguration(),
    ];

    internal static void Apply(JsonTypeInfo typeInfo)
    {
        if (typeInfo.Kind != JsonTypeInfoKind.Object)
        {
            return;
        }

        foreach (var convention in Conventions)
        {
            convention.Configure(typeInfo);
        }

        foreach (var configuration in AggregateConfigurations)
        {
            configuration.Configure(typeInfo);
        }
    }
}
