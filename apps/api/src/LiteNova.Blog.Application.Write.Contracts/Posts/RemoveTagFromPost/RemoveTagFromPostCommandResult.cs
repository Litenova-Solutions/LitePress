namespace LiteNova.Blog.Application.Write.Contracts.Posts.RemoveTagFromPost;

public sealed record RemoveTagFromPostCommandResult(Guid PostId, Guid TagId);
