namespace LiteNova.Blog.Application.Common.Exceptions;
public sealed class DuplicateSlugException(string slug) : ApplicationException($"Duplicate slug detected: {slug}");
