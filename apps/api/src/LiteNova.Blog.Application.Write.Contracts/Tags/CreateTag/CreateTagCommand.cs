namespace LiteNova.Blog.Application.Write.Contracts.Tags.CreateTag;

public sealed record CreateTagCommand : ICommand<CreateTagCommandResult>
{
    public required TagId TagId { get; init; }
    public required string Name { get; init; }
}
