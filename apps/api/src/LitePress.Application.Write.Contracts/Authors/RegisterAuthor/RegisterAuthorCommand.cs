namespace LitePress.Application.Write.Contracts.Authors.RegisterAuthor;

public sealed record RegisterAuthorCommand : ICommand<RegisterAuthorCommandResult>
{
    public required AuthorId AuthorId { get; init; }
    public required string ExternalId { get; init; }
    public required string DisplayName { get; init; }
}
