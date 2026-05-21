namespace LiteNova.Blog.Application.Write.Contracts.Tags.CreateTag;

public sealed record CreateTagCommandResult(Guid TagId, string Slug);
