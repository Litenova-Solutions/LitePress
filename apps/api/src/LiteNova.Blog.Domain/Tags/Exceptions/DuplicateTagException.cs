using LiteNova.Blog.Domain.Common;
namespace LiteNova.Blog.Domain.Tags.Exceptions;
public sealed class DuplicateTagException(string slug) : DomainException($"Tag with slug {slug} already exists.");
