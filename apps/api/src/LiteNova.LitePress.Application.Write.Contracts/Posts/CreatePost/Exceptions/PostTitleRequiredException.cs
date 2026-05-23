using LiteNova.LitePress.Application.Write.Contracts.Shared.Exceptions;

namespace LiteNova.LitePress.Application.Write.Contracts.Posts.CreatePost.Exceptions;

public sealed class PostTitleRequiredException : CommandValidationException
{
    public PostTitleRequiredException()
        : base("A post title is required and cannot be empty.") { }
}
