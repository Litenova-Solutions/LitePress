namespace LiteNova.LitePress.Application.Write.Contracts.Posts.PublishPost;

public sealed record PublishPostCommand : ICommand<PublishPostCommandResult>
{
    public required PostId PostId { get; init; }
}
