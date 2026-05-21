using LiteNova.Blog.Domain.Shared.Exceptions;

namespace LiteNova.Blog.Domain.Posts.Exceptions;

public sealed class PostNotEditableException : DomainException
{
    public PostNotEditableException(PostId id)
        : base($"Post ''{id.Value}'' cannot be edited because it is not in Draft state.") { }
}
