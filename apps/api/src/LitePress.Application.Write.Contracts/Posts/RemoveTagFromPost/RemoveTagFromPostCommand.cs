namespace LitePress.Application.Write.Contracts.Posts.RemoveTagFromPost;

public sealed record RemoveTagFromPostCommand : ICommand<RemoveTagFromPostCommandResult>
{
    public required PostId PostId { get; init; }
    public required TagId TagId { get; init; }
}
