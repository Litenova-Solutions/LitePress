using LiteNova.Blog.Domain.Shared.Exceptions;

namespace LiteNova.Blog.Domain.Authors.Exceptions;

public sealed class AuthorNotFoundException : AggregateNotFoundException
{
    public AuthorNotFoundException(AuthorId id)
        : base($"Author ''{id.Value}'' was not found.") { }
}
