using LitePress.Domain.Common;
namespace LitePress.Domain.Tags.Exceptions;
public sealed class DuplicateTagException(string slug) : DomainException($"Tag with slug {slug} already exists.");
