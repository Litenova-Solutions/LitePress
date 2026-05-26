using LitePress.Domain.Shared.Exceptions;

namespace LitePress.Domain.Posts.Exceptions;

/// <summary>Thrown when a title cannot produce a valid slug.</summary>
public sealed class InvalidPostSlugException(string title)
    : DomainException($"Unable to generate slug from title: {title}");
