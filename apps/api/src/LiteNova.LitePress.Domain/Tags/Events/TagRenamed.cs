using LiteNova.LitePress.Domain.Shared;

namespace LiteNova.LitePress.Domain.Tags.Events;

public sealed record TagRenamed(TagId TagId, TagName NewName, TagSlug NewSlug) : IDomainEvent;
