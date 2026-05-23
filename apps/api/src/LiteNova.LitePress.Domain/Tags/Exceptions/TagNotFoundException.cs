using LiteNova.LitePress.Domain.Shared.Exceptions;

namespace LiteNova.LitePress.Domain.Tags.Exceptions;

public sealed class TagNotFoundException : AggregateNotFoundException
{
    public TagNotFoundException(TagId id)
        : base($"Tag ''{id.Value}'' was not found.") { }
}
