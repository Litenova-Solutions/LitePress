namespace LitePress.Application.Write.Contracts.Posts.ArchivePost;

public sealed record ArchivePostCommand : ICommand<ArchivePostCommandResult>
{
    public required PostId PostId { get; init; }
}
