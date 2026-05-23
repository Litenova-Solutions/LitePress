using LiteNova.LitePress.Application.Write.Contracts.Shared.Exceptions;

namespace LiteNova.LitePress.Application.Write.Contracts.Posts.CreatePost.Exceptions;

public sealed class PostContentRequiredException : CommandValidationException
{
    public PostContentRequiredException()
        : base("Post content is required and cannot be empty.") { }
}
