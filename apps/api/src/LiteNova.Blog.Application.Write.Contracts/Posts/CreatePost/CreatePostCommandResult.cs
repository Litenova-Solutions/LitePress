namespace LiteNova.Blog.Application.Write.Contracts.Posts.CreatePost;

public sealed record CreatePostCommandResult(Guid PostId, string Slug);
