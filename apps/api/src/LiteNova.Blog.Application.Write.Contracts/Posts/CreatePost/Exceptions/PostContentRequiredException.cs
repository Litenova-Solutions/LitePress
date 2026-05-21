using LiteNova.Blog.Application.Write.Contracts.Shared.Exceptions;

namespace LiteNova.Blog.Application.Write.Contracts.Posts.CreatePost.Exceptions;

public sealed class PostContentRequiredException : CommandValidationException
{
    public PostContentRequiredException()
        : base("Post content is required and cannot be empty.") { }
}
