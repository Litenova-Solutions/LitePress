using LiteNova.LitePress.Domain.Common;
namespace LiteNova.LitePress.Domain.Tags.Exceptions;
public sealed class DuplicateTagException(string slug) : DomainException($"Tag with slug {slug} already exists.");
