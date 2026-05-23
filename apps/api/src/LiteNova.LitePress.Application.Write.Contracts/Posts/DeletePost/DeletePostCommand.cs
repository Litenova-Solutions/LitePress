namespace LiteNova.LitePress.Application.Write.Contracts.Posts.DeletePost;

public sealed record DeletePostCommand : ICommand<DeletePostCommandResult>
{
    public required PostId PostId { get; init; }
}
