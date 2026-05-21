namespace LiteNova.Blog.Application.Write.Contracts.Posts.UpdatePost;

public sealed record UpdatePostCommandResult(Guid PostId, string Slug);
