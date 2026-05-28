using LitePress.Domain.Shared;
using LitePress.Infrastructure.Marten.Serialization.Internal;
using System.Text.Json.Serialization.Metadata;

namespace LitePress.Infrastructure.Marten.Serialization.Abstractions.Configurations;

/// <summary>
/// Base configuration for an aggregate root and optional related types stored in the same JSON document.
/// </summary>
internal abstract class AggregateRootJsonConfiguration<TAggregate> : IAggregateRootJsonConfiguration
    where TAggregate : class, IAggregateRoot
{
    public Type AggregateRootType => typeof(TAggregate);

    protected virtual IReadOnlyList<Type> RelatedTypes => [];

    public void Configure(JsonTypeInfo typeInfo)
    {
        if (typeInfo.Type == typeof(TAggregate))
        {
            ConfigureAggregateRoot(typeInfo);
            return;
        }

        foreach (var relatedType in RelatedTypes)
        {
            if (typeInfo.Type == relatedType)
            {
                ConfigureRelatedType(typeInfo, relatedType);
                return;
            }
        }
    }

    protected virtual void ConfigureAggregateRoot(JsonTypeInfo typeInfo)
    {
        var clrType = typeof(TAggregate);
        typeInfo.UseNonPublicConstructorIfPresent(clrType);
        typeInfo.EnableNonPublicPropertySetters(clrType);
        typeInfo.IgnoreProperty(nameof(IAggregateRoot.DomainEvents));
    }

    protected virtual void ConfigureRelatedType(JsonTypeInfo typeInfo, Type relatedType)
    {
    }
}
