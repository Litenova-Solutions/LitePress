using System.Text.Json.Serialization.Metadata;
using Marten;
using Weasel.Core;

namespace LitePress.Infrastructure.Marten.Serialization.Internal;

/// <summary>
/// Registers Marten System.Text.Json serialization and project-specific type rules.
/// </summary>
internal static class MartenJsonSerializerConfigurer
{
    internal static void Configure(StoreOptions options)
    {
        options.UseSystemTextJsonForSerialization(
            EnumStorage.AsString,
            Casing.Default,
            serializerOptions =>
            {
                serializerOptions.PropertyNamingPolicy = null;
                serializerOptions.IncludeFields = true;
                serializerOptions.TypeInfoResolver = new DefaultJsonTypeInfoResolver
                {
                    Modifiers = { JsonTypeConfigurationRegistry.Apply }
                };
            });
    }
}
