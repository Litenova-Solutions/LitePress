using LiteNova.Blog.Application.Write.Contracts.Tags.RenameTag;
using LiteNova.Blog.Application.Write.Contracts.Tags.RenameTag.Exceptions;

namespace LiteNova.Blog.Application.Write.Tags.Rename;

internal sealed class RenameTagCommandValidator : ICommandValidator<RenameTagCommand>
{
    public Task ValidateAsync(RenameTagCommand command, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(command.NewName))
        {
            throw new TagNameRequiredException();
        }

        if (command.NewName.Length > 50)
        {
            throw new TagNameTooLongException(command.NewName.Length);
        }

        return Task.CompletedTask;
    }
}
