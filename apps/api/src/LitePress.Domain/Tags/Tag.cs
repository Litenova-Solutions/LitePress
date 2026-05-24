using LitePress.Domain.Shared;
using LitePress.Domain.Tags.Events;

namespace LitePress.Domain.Tags;

public sealed class Tag : AggregateRoot<TagId>
{
    private Tag() { }

    public TagName Name { get; private set; } = null!;
    public TagSlug Slug { get; private set; } = null!;
    public DateTimeOffset CreatedAt { get; private set; }

    public static Tag Create(TagId id, TagName name)
    {
        var slug = TagSlug.FromName(name.Value);
        var tag = new Tag
        {
            Id = id,
            Name = name,
            Slug = slug,
            CreatedAt = DateTimeOffset.UtcNow
        };
        tag.RaiseDomainEvent(new TagCreated(id, name, slug));
        return tag;
    }

    public void Rename(TagName newName)
    {
        Name = newName;
        Slug = TagSlug.FromName(newName.Value);
        RaiseDomainEvent(new TagRenamed(Id, newName, Slug));
    }

    public void Delete() => RaiseDomainEvent(new TagDeleted(Id));
}
