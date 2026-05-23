namespace LiteNova.LitePress.Application.Write.Contracts.Tags.DeleteTag;

public sealed record DeleteTagCommand : ICommand<DeleteTagCommandResult>
{
    public required TagId TagId { get; init; }
}
