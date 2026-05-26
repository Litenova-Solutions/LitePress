using LitePress.Application.Write.Contracts.Authors.RegisterAuthor;
using LitePress.Application.Write.Contracts.Shared;

namespace LitePress.Application.Write.Authors.Register;

internal sealed class RegisterAuthorCommandHandler : ICommandHandler<RegisterAuthorCommand, RegisterAuthorCommandResult>
{
    private readonly IAuthorRepository _authorRepository;
    private readonly IClock _clock;

    public RegisterAuthorCommandHandler(IAuthorRepository authorRepository, IClock clock)
    {
        _authorRepository = authorRepository;
        _clock = clock;
    }

    public async Task<RegisterAuthorCommandResult> HandleAsync(RegisterAuthorCommand command, CancellationToken cancellationToken)
    {
        var existing = await _authorRepository.FindByExternalIdAsync(command.ExternalId, cancellationToken);
        if (existing is not null)
        {
            return new RegisterAuthorCommandResult(existing.Id.Value);
        }

        var author = Author.Register(
            command.AuthorId,
            command.ExternalId,
            command.DisplayName,
            _clock.UtcNow);

        await _authorRepository.AddAsync(author, cancellationToken);

        return new RegisterAuthorCommandResult(author.Id.Value);
    }
}
