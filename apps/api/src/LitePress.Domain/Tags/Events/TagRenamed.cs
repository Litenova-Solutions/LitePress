using LitePress.Domain.Shared;

namespace LitePress.Domain.Tags.Events;

public sealed record TagRenamed(TagId TagId, TagName NewName, TagSlug NewSlug) : IDomainEvent;
