using LiteNova.LitePress.Domain.Shared.Exceptions;

namespace LiteNova.LitePress.Domain.Posts.Exceptions;

public sealed class PostCannotBeDeletedException : DomainException
{
    public PostCannotBeDeletedException(PostId id)
        : base($"Post ''{id.Value}'' cannot be deleted because it is published. Archive it first.") { }
}
