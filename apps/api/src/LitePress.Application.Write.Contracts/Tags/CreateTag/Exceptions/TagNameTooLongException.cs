using LitePress.Application.Write.Contracts.Shared.Exceptions;

namespace LitePress.Application.Write.Contracts.Tags.CreateTag.Exceptions;

public sealed class TagNameTooLongException : CommandValidationException
{
    public TagNameTooLongException(int length)
        : base($"Tag name is {length} characters, which exceeds the 50 character limit.") { }
}
