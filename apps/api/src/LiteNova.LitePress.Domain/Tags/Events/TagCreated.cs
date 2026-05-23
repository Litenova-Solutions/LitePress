using LiteNova.LitePress.Domain.Shared;

namespace LiteNova.LitePress.Domain.Tags.Events;

public sealed record TagCreated(TagId TagId, TagName Name, TagSlug Slug) : IDomainEvent;
