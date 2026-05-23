namespace LiteNova.LitePress.Application.Write.Contracts.Posts.CreatePost;

public sealed record CreatePostCommandResult(Guid PostId, string Slug);
