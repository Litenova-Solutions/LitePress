using LiteNova.Blog.Domain.Shared.Exceptions;

namespace LiteNova.Blog.Domain.Tags.Exceptions;

public sealed class TagNotFoundException : AggregateNotFoundException
{
    public TagNotFoundException(TagId id)
        : base($"Tag ''{id.Value}'' was not found.") { }
}
