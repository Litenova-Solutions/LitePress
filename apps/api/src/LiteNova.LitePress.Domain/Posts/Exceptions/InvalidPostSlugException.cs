using LiteNova.LitePress.Domain.Common;
namespace LiteNova.LitePress.Domain.Posts.Exceptions;
public sealed class InvalidPostSlugException(string title) : DomainException($"Unable to generate slug from title: {title}");
