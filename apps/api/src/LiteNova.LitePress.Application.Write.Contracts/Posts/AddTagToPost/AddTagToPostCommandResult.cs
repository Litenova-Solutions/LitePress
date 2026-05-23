namespace LiteNova.LitePress.Application.Write.Contracts.Posts.AddTagToPost;

public sealed record AddTagToPostCommandResult(Guid PostId, Guid TagId);
