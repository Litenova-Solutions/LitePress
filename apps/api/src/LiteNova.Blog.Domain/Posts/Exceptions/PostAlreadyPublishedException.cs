using LiteNova.Blog.Domain.Shared.Exceptions;

namespace LiteNova.Blog.Domain.Posts.Exceptions;

public sealed class PostAlreadyPublishedException : DomainException
{
    public PostAlreadyPublishedException(PostId id)
        : base($"Post ''{id.Value}'' is already published.") { }
}
