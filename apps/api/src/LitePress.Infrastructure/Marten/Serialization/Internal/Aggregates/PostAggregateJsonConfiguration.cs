using LitePress.Domain.Posts;
using LitePress.Infrastructure.Marten.Serialization.Abstractions.Configurations;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace LitePress.Infrastructure.Marten.Serialization.Internal.Aggregates;

/// <summary>
/// JSON rules for the <see cref="Post"/> document, including <c>_tags</c> and <see cref="PostState"/> polymorphism.
/// </summary>
internal sealed class PostAggregateJsonConfiguration : AggregateRootJsonConfiguration<Post>
{
    protected override IReadOnlyList<Type> RelatedTypes => [typeof(PostState)];

    protected override void ConfigureAggregateRoot(JsonTypeInfo typeInfo)
    {
        base.ConfigureAggregateRoot(typeInfo);
        typeInfo.IncludeNonPublicInstanceFields(typeof(Post));
    }

    protected override void ConfigureRelatedType(JsonTypeInfo typeInfo, Type relatedType)
    {
        if (relatedType != typeof(PostState))
        {
            return;
        }

        typeInfo.PolymorphismOptions = new JsonPolymorphismOptions
        {
            TypeDiscriminatorPropertyName = "$type",
            UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization,
            DerivedTypes =
            {
                new JsonDerivedType(typeof(DraftPostState), "Draft"),
                new JsonDerivedType(typeof(PublishedPostState), "Published"),
                new JsonDerivedType(typeof(ArchivedPostState), "Archived"),
            }
        };
    }
}
