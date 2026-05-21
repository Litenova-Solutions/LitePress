using LiteNova.Blog.Application.Write.Contracts.Shared.Exceptions;

namespace LiteNova.Blog.Application.Write.Contracts.Tags.RenameTag.Exceptions;

public sealed class TagNameTooLongException : CommandValidationException
{
    public TagNameTooLongException(int length)
        : base($"Tag name is {length} characters, which exceeds the 50 character limit.") { }
}
