using LitePress.Domain.Shared.Exceptions;
namespace LitePress.Domain.Tags.Exceptions;
public sealed class DuplicateTagException(string slug) : DomainException($"Tag with slug {slug} already exists.");
