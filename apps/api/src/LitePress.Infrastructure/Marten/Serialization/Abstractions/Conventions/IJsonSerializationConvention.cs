using System.Text.Json.Serialization.Metadata;

namespace LitePress.Infrastructure.Marten.Serialization.Abstractions.Conventions;

/// <summary>
/// Cross-cutting JSON rules applied to every matching CLR type.
/// </summary>
internal interface IJsonSerializationConvention
{
    public bool AppliesTo(Type type);

    public void Configure(JsonTypeInfo typeInfo);
}
