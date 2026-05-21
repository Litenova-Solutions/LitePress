namespace LiteNova.Blog.Application.Write.Contracts.Posts.AddTagToPost;

public sealed record AddTagToPostCommandResult(Guid PostId, Guid TagId);
