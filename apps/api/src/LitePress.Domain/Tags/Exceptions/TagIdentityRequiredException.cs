namespace LitePress.Domain.Tags.Exceptions;

using LitePress.Domain.Shared.Exceptions;

/// <summary>Thrown when a tag id is missing during creation.</summary>
public sealed class TagIdentityRequiredException()
    : DomainException("Tag id is required.");
