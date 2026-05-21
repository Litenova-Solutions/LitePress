using LiteNova.Blog.Application.Write.Contracts.Shared.Exceptions;

namespace LiteNova.Blog.Application.Write.Contracts.Posts.CreatePost.Exceptions;

public sealed class PostTitleTooLongException : CommandValidationException
{
    public PostTitleTooLongException(int length)
        : base($"Post title is {length} characters, which exceeds the 200 character limit.") { }
}
