using LiteNova.Blog.Application.Write.Contracts.Tags.RenameTag;
using LiteNova.Blog.Application.Write.Contracts.Tags.RenameTag.Exceptions;
using LiteNova.Blog.Domain.Tags.Exceptions;

namespace LiteNova.Blog.Application.Write.Tags.Rename;

internal sealed class RenameTagCommandHandler : ICommandHandler<RenameTagCommand, RenameTagCommandResult>
{
    private readonly ITagRepository _tagRepository;

    public RenameTagCommandHandler(ITagRepository tagRepository)
    {
        _tagRepository = tagRepository;
    }

    public async Task<RenameTagCommandResult> HandleAsync(RenameTagCommand command, CancellationToken cancellationToken)
    {
        var newName = new TagName(command.NewName);

        if (await _tagRepository.NameExistsAsync(newName, cancellationToken))
        {
            throw new TagNameAlreadyExistsException(newName);
        }

        var tag = await _tagRepository.GetByIdAsync(command.TagId, cancellationToken);
        tag.Rename(newName);
        await _tagRepository.UpdateAsync(tag, cancellationToken);

        return new RenameTagCommandResult(tag.Id.Value, tag.Slug.Value);
    }
}
