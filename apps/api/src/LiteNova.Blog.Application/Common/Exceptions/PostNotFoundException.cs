namespace LiteNova.Blog.Application.Common.Exceptions;

public sealed class PostNotFoundException : ApplicationException
{
    public PostNotFoundException(Guid id)
        : base($"Post with id {id} was not found.")
    {
    }

    public PostNotFoundException(string slug)
        : base($"Post with slug '{slug}' was not found.")
    {
    }
}
