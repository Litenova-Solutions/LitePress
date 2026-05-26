namespace LitePress.Domain.Authors.Exceptions;

using LitePress.Domain.Shared.Exceptions;

/// <summary>Thrown when an author id is missing during registration.</summary>
public sealed class AuthorIdentityRequiredException()
    : DomainException("Author id is required.");
