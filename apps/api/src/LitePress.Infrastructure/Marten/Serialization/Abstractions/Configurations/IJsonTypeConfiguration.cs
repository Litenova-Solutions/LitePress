using System.Text.Json.Serialization.Metadata;

namespace LitePress.Infrastructure.Marten.Serialization.Abstractions.Configurations;

/// <summary>
/// Configures System.Text.Json metadata for a persisted CLR type.
/// </summary>
internal interface IJsonTypeConfiguration
{
    public void Configure(JsonTypeInfo typeInfo);
}
