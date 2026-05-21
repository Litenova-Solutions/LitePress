using LiteNova.Blog.Domain.Shared.Exceptions;

namespace LiteNova.Blog.Domain.Posts.Exceptions;

public sealed class PostNotFoundException : AggregateNotFoundException
{
    public PostNotFoundException(PostId id)
        : base($"Post ''{id.Value}'' was not found.") { }
}
