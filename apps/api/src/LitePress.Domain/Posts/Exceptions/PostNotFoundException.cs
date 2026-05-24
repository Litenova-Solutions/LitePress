using LitePress.Domain.Shared.Exceptions;

namespace LitePress.Domain.Posts.Exceptions;

public sealed class PostNotFoundException : AggregateNotFoundException
{
    public PostNotFoundException(PostId id)
        : base($"Post ''{id.Value}'' was not found.") { }
}
