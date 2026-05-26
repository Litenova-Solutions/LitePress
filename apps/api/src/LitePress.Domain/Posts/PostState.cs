namespace LitePress.Domain.Posts;

/// <summary>Discriminated union of post lifecycle states.</summary>
public abstract record PostState;

/// <summary>Post is a draft and can be edited.</summary>
public sealed record DraftPostState : PostState;

/// <summary>Post is published and visible to readers.</summary>
/// <param name="PublishedAt">When the post was published.</param>
public sealed record PublishedPostState(DateTimeOffset PublishedAt) : PostState;

/// <summary>Post is archived and no longer actively published.</summary>
/// <param name="ArchivedAt">When the post was archived.</param>
public sealed record ArchivedPostState(DateTimeOffset ArchivedAt) : PostState;
