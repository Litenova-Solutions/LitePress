using LitePress.Application.Write.Contracts.Shared.Exceptions;

namespace LitePress.Application.Write.Contracts.Tags.CreateTag.Exceptions;

public sealed class TagNameRequiredException : CommandValidationException
{
    public TagNameRequiredException()
        : base("A tag name is required and cannot be empty.") { }
}
