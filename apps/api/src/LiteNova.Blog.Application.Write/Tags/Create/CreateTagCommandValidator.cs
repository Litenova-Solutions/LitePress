using LiteNova.Blog.Application.Write.Contracts.Tags.CreateTag;
using LiteNova.Blog.Application.Write.Contracts.Tags.CreateTag.Exceptions;

namespace LiteNova.Blog.Application.Write.Tags.Create;

internal sealed class CreateTagCommandValidator : ICommandValidator<CreateTagCommand>
{
    public Task ValidateAsync(CreateTagCommand command, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(command.Name))
        {
            throw new TagNameRequiredException();
        }

        if (command.Name.Length > 50)
        {
            throw new TagNameTooLongException(command.Name.Length);
        }

        return Task.CompletedTask;
    }
}
