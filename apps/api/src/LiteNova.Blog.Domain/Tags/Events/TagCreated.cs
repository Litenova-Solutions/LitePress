using LiteNova.Blog.Domain.Shared;

namespace LiteNova.Blog.Domain.Tags.Events;

public sealed record TagCreated(TagId TagId, TagName Name, TagSlug Slug) : IDomainEvent;
