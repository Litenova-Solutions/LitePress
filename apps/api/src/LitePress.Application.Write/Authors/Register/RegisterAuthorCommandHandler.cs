using LitePress.Application.Write.Contracts.Authors.RegisterAuthor;

namespace LitePress.Application.Write.Authors.Register;

internal sealed class RegisterAuthorCommandHandler : ICommandHandler<RegisterAuthorCommand, RegisterAuthorCommandResult>
{
    private readonly IAuthorRepository _authorRepository;

    public RegisterAuthorCommandHandler(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    public async Task<RegisterAuthorCommandResult> HandleAsync(RegisterAuthorCommand command, CancellationToken cancellationToken)
    {
        var existing = await _authorRepository.FindByExternalIdAsync(command.ExternalId, cancellationToken);
        if (existing is not null)
        {
            return new RegisterAuthorCommandResult(existing.Id.Value);
        }

        var author = Author.Register(command.AuthorId, command.ExternalId, command.DisplayName);
        await _authorRepository.AddAsync(author, cancellationToken);

        return new RegisterAuthorCommandResult(author.Id.Value);
    }
}
