using LiteNova.LitePress.Domain.Shared.Exceptions;

namespace LiteNova.LitePress.Domain.Posts.Exceptions;

public sealed class PostAlreadyArchivedException : DomainException
{
    public PostAlreadyArchivedException(PostId id)
        : base($"Post ''{id.Value}'' is already archived.") { }
}
