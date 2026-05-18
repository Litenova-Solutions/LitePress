namespace LiteNova.Blog.Application.Common.Exceptions;
public sealed class TagNotFoundException(Guid id) : ApplicationException($"Tag with id {id} was not found.");
