using LiteNova.Blog.Domain.Common;
namespace LiteNova.Blog.Domain.Posts.Exceptions;
public sealed class InvalidPostSlugException(string title) : DomainException($"Unable to generate slug from title: {title}");
