namespace LitePress.Application.Write.Contracts.Posts.UpdatePost;

public sealed record UpdatePostCommand : ICommand<UpdatePostCommandResult>
{
    public required PostId PostId { get; init; }
    public required string Title { get; init; }
    public required string Content { get; init; }
    public string? Excerpt { get; init; }
    public string? CoverImageUrl { get; init; }
}
