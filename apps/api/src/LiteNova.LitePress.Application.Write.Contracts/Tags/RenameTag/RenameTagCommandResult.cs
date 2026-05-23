namespace LiteNova.LitePress.Application.Write.Contracts.Tags.RenameTag;

public sealed record RenameTagCommandResult(Guid TagId, string Slug);
