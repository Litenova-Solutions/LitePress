using LiteNova.LitePress.Domain.Shared.Exceptions;

namespace LiteNova.LitePress.Domain.Tags.Exceptions;

public sealed class TagNameAlreadyExistsException : DomainException
{
    public TagNameAlreadyExistsException(TagName name)
        : base($"A tag with name ''{name.Value}'' already exists.") { }
}
