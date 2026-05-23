using LiteNova.LitePress.Application.Write.Contracts.Tags.CreateTag;
using LiteNova.LitePress.Application.Write.Contracts.Tags.CreateTag.Exceptions;
using LiteNova.LitePress.Domain.Tags.Exceptions;

namespace LiteNova.LitePress.Application.Write.Tags.Create;

internal sealed class CreateTagCommandHandler : ICommandHandler<CreateTagCommand, CreateTagCommandResult>
{
    private readonly ITagRepository _tagRepository;

    public CreateTagCommandHandler(ITagRepository tagRepository)
    {
        _tagRepository = tagRepository;
    }

    public async Task<CreateTagCommandResult> HandleAsync(CreateTagCommand command, CancellationToken cancellationToken)
    {
        var name = new TagName(command.Name);

        if (await _tagRepository.NameExistsAsync(name, cancellationToken))
        {
            throw new TagNameAlreadyExistsException(name);
        }

        var tag = Tag.Create(command.TagId, name);
        await _tagRepository.AddAsync(tag, cancellationToken);

        return new CreateTagCommandResult(tag.Id.Value, tag.Slug.Value);
    }
}
