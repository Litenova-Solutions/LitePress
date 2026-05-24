namespace LitePress.Application.Write.Contracts.Posts.AddTagToPost;

public sealed record AddTagToPostCommand : ICommand<AddTagToPostCommandResult>
{
    public required PostId PostId { get; init; }
    public required TagId TagId { get; init; }
}
