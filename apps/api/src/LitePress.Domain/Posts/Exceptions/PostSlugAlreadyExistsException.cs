using LitePress.Domain.Shared.Exceptions;

namespace LitePress.Domain.Posts.Exceptions;

public sealed class PostSlugAlreadyExistsException : DomainException
{
    public PostSlugAlreadyExistsException(PostSlug slug)
        : base($"A post with slug ''{slug.Value}'' already exists.") { }
}
