using LiteNova.Blog.Domain.Shared.Exceptions;

namespace LiteNova.Blog.Domain.Posts.Exceptions;

public sealed class PostAlreadyArchivedException : DomainException
{
    public PostAlreadyArchivedException(PostId id)
        : base($"Post ''{id.Value}'' is already archived.") { }
}
