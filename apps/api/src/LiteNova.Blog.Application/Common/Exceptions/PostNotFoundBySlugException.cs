namespace LiteNova.Blog.Application.Common.Exceptions;
public sealed class PostNotFoundBySlugException(string slug) : ApplicationException($"Post with slug '{slug}' was not found.");
