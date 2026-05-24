using LitePress.Domain.Shared.Exceptions;

namespace LitePress.Domain.Tags.Exceptions;

public sealed class TagNotFoundException : AggregateNotFoundException
{
    public TagNotFoundException(TagId id)
        : base($"Tag ''{id.Value}'' was not found.") { }
}
