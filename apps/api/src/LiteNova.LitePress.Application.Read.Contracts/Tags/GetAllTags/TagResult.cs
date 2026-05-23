namespace LiteNova.LitePress.Application.Read.Contracts.Tags.GetAllTags;

public sealed record TagResult(Guid TagId, string Name, string Slug, int PostCount);
