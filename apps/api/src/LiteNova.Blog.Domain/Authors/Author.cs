using LiteNova.Blog.Domain.Authors.Events;
using LiteNova.Blog.Domain.Shared;

namespace LiteNova.Blog.Domain.Authors;

public sealed class Author : AggregateRoot<AuthorId>
{
    private Author() { }

    public string DisplayName { get; private set; } = null!;
    public string ExternalId { get; private set; } = null!;
    public DateTimeOffset RegisteredAt { get; private set; }

    public static Author Register(AuthorId id, string externalId, string displayName)
    {
        var author = new Author
        {
            Id = id,
            ExternalId = externalId,
            DisplayName = displayName,
            RegisteredAt = DateTimeOffset.UtcNow
        };
        author.RaiseDomainEvent(new AuthorRegistered(id, displayName));
        return author;
    }
}
