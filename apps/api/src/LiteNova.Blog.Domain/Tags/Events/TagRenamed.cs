using LiteNova.Blog.Domain.Shared;

namespace LiteNova.Blog.Domain.Tags.Events;

public sealed record TagRenamed(TagId TagId, TagName NewName, TagSlug NewSlug) : IDomainEvent;
