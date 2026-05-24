using LitePress.Domain.Shared.Exceptions;

namespace LitePress.Domain.Posts.Exceptions;

public sealed class PostTagLimitExceededException : DomainException
{
    public PostTagLimitExceededException(PostId id)
        : base($"Post ''{id.Value}'' already has the maximum of 10 tags.") { }
}
