using LitePress.Domain.Shared;
using LitePress.Infrastructure.Marten.Serialization.Abstractions.Conventions;
using System.Text.Json.Serialization.Metadata;

namespace LitePress.Infrastructure.Marten.Serialization.Internal.Conventions;

/// <summary>
/// Enables deserialization of domain value types and nested objects with private constructors and setters.
/// Aggregate roots are configured separately; this convention skips them to avoid duplicate work.
/// </summary>
internal sealed class DomainTypeSerializationConvention : JsonSerializationConvention
{
    public override bool AppliesTo(Type type) =>
        type.IsClass
        && type.Namespace?.StartsWith("LitePress.Domain", StringComparison.Ordinal) == true
        && !typeof(IAggregateRoot).IsAssignableFrom(type);

    protected override void ConfigureType(JsonTypeInfo typeInfo)
    {
        var clrType = typeInfo.Type;
        typeInfo.UseNonPublicConstructorIfPresent(clrType);
        typeInfo.EnableNonPublicPropertySetters(clrType);
    }
}
