using LiteNova.LitePress.Domain.Shared.Exceptions;

namespace LiteNova.LitePress.Domain.Posts.Exceptions;

public sealed class PostAlreadyPublishedException : DomainException
{
    public PostAlreadyPublishedException(PostId id)
        : base($"Post ''{id.Value}'' is already published.") { }
}
