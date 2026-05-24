using LitePress.Domain.Shared.Exceptions;

namespace LitePress.Domain.Authors.Exceptions;

public sealed class AuthorNotFoundException : AggregateNotFoundException
{
    public AuthorNotFoundException(AuthorId id)
        : base($"Author ''{id.Value}'' was not found.") { }
}
