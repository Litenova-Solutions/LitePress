using LiteNova.Blog.Application.Write.Contracts.Shared.Exceptions;

namespace LiteNova.Blog.Application.Write.Contracts.Tags.RenameTag.Exceptions;

public sealed class TagNameRequiredException : CommandValidationException
{
    public TagNameRequiredException()
        : base("A tag name is required and cannot be empty.") { }
}
