namespace LiteNova.LitePress.Application.Write.Contracts.Posts.UpdatePost;

public sealed record UpdatePostCommandResult(Guid PostId, string Slug);
