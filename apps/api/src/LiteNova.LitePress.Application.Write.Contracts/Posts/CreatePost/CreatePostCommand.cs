namespace LiteNova.LitePress.Application.Write.Contracts.Posts.CreatePost;

public sealed record CreatePostCommand : ICommand<CreatePostCommandResult>
{
    public required PostId PostId { get; init; }
    public required AuthorId AuthorId { get; init; }
    public required string Title { get; init; }
    public required string Content { get; init; }
    public string? Excerpt { get; init; }
    public string? CoverImageUrl { get; init; }
    public IReadOnlyList<Guid> TagIds { get; init; } = [];
}
