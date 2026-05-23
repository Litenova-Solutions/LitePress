using LiteNova.LitePress.Application.Write.Contracts.Tags.CreateTag;
using LiteNova.LitePress.Application.Write.Contracts.Tags.CreateTag.Exceptions;

namespace LiteNova.LitePress.Application.Write.Tags.Create;

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
