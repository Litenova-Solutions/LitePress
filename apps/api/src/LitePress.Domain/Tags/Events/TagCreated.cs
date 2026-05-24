using LitePress.Domain.Shared;

namespace LitePress.Domain.Tags.Events;

public sealed record TagCreated(TagId TagId, TagName Name, TagSlug Slug) : IDomainEvent;
