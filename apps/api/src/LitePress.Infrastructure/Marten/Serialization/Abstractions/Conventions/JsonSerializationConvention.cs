using System.Text.Json.Serialization.Metadata;

namespace LitePress.Infrastructure.Marten.Serialization.Abstractions.Conventions;

/// <summary>
/// Base class for conventions that configure JSON metadata for types matching <see cref="AppliesTo"/>.
/// </summary>
internal abstract class JsonSerializationConvention : IJsonSerializationConvention
{
    public abstract bool AppliesTo(Type type);

    public void Configure(JsonTypeInfo typeInfo)
    {
        if (!AppliesTo(typeInfo.Type))
        {
            return;
        }

        ConfigureType(typeInfo);
    }

    protected abstract void ConfigureType(JsonTypeInfo typeInfo);
}
