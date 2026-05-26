using LitePress.Domain.Authors.Events;
using LitePress.Domain.Authors.Exceptions;
using LitePress.Domain.Shared;

namespace LitePress.Domain.Authors;

/// <summary>Author aggregate root.</summary>
public sealed class Author : AggregateRoot<AuthorId>
{
    private Author() { }

    /// <summary>Display name shown in the admin and public site.</summary>
    public string DisplayName { get; private set; } = null!;

    /// <summary>External identity provider subject id.</summary>
    public string ExternalId { get; private set; } = null!;

    /// <summary>When the author registered in LitePress.</summary>
    public DateTimeOffset RegisteredAt { get; private set; }

    /// <summary>Registers a new author.</summary>
    /// <param name="utcNow">Current UTC time from the handler.</param>
    public static Author Register(
        AuthorId id,
        string externalId,
        string displayName,
        DateTimeOffset utcNow)
    {
        if (id == default)
        {
            throw new AuthorIdentityRequiredException();
        }

        var author = new Author
        {
            Id = id,
            ExternalId = externalId,
            DisplayName = displayName,
            RegisteredAt = utcNow
        };

        author.RaiseDomainEvent(new AuthorRegistered(id, displayName));
        return author;
    }
}
