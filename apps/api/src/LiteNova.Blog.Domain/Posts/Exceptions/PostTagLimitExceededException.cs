using LiteNova.Blog.Domain.Shared.Exceptions;

namespace LiteNova.Blog.Domain.Posts.Exceptions;

public sealed class PostTagLimitExceededException : DomainException
{
    public PostTagLimitExceededException(PostId id)
        : base($"Post ''{id.Value}'' already has the maximum of 10 tags.") { }
}
