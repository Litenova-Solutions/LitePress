using LiteNova.LitePress.Domain.Shared.Exceptions;

namespace LiteNova.LitePress.Domain.Posts.Exceptions;

public sealed class PostSlugAlreadyExistsException : DomainException
{
    public PostSlugAlreadyExistsException(PostSlug slug)
        : base($"A post with slug ''{slug.Value}'' already exists.") { }
}
