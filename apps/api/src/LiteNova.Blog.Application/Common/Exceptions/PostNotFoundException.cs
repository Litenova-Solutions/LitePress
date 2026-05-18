namespace LiteNova.Blog.Application.Common.Exceptions;
public sealed class PostNotFoundException(Guid id) : ApplicationException($"Post with id {id} was not found.");
