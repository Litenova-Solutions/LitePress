namespace LitePress.Domain.Posts.Exceptions;

using LitePress.Domain.Shared.Exceptions;

/// <summary>Thrown when a post id is missing during creation.</summary>
public sealed class PostIdentityRequiredException()
    : DomainException("Post id is required.");
