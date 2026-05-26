using LitePress.Domain.Shared;
using LitePress.Domain.Tags.Events;
using LitePress.Domain.Tags.Exceptions;

namespace LitePress.Domain.Tags;

/// <summary>Tag aggregate root.</summary>
public sealed class Tag : AggregateRoot<TagId>
{
    private Tag() { }

    /// <summary>Display name of the tag.</summary>
    public TagName Name { get; private set; } = null!;

    /// <summary>URL slug derived from the tag name.</summary>
    public TagSlug Slug { get; private set; } = null!;

    /// <summary>When the tag was created.</summary>
    public DateTimeOffset CreatedAt { get; private set; }

    /// <summary>Creates a new tag.</summary>
    /// <param name="utcNow">Current UTC time from the handler.</param>
    public static Tag Create(TagId id, TagName name, DateTimeOffset utcNow)
    {
        if (id == default)
        {
            throw new TagIdentityRequiredException();
        }

        var slug = TagSlug.FromName(name.Value);
        var tag = new Tag
        {
            Id = id,
            Name = name,
            Slug = slug,
            CreatedAt = utcNow
        };

        tag.RaiseDomainEvent(new TagCreated(id, name, slug));
        return tag;
    }

    /// <summary>Renames the tag and regenerates its slug.</summary>
    public void Rename(TagName newName)
    {
        Name = newName;
        Slug = TagSlug.FromName(newName.Value);
        RaiseDomainEvent(new TagRenamed(Id, newName, Slug));
    }

    /// <summary>Marks the tag for deletion.</summary>
    public void Delete() =>
        RaiseDomainEvent(new TagDeleted(Id));
}
