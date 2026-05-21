namespace LiteNova.Blog.Application.Write.Contracts.Tags.RenameTag;

public sealed record RenameTagCommand : ICommand<RenameTagCommandResult>
{
    public required TagId TagId { get; init; }
    public required string NewName { get; init; }
}
