namespace LitePress.Domain.Posts;

public abstract record PostState;
public sealed record DraftPostState : PostState;
public sealed record PublishedPostState(DateTimeOffset PublishedAt) : PostState;
public sealed record ArchivedPostState(DateTimeOffset ArchivedAt) : PostState;
