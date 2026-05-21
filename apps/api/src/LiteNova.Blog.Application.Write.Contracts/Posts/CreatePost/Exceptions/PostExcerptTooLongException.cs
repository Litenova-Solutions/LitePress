using LiteNova.Blog.Application.Write.Contracts.Shared.Exceptions;

namespace LiteNova.Blog.Application.Write.Contracts.Posts.CreatePost.Exceptions;

public sealed class PostExcerptTooLongException : CommandValidationException
{
    public PostExcerptTooLongException(int length)
        : base($"Post excerpt is {length} characters, which exceeds the 500 character limit.") { }
}
