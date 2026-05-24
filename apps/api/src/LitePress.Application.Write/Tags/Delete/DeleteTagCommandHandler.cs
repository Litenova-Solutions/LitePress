using LitePress.Application.Write.Contracts.Tags.DeleteTag;

namespace LitePress.Application.Write.Tags.Delete;

internal sealed class DeleteTagCommandHandler : ICommandHandler<DeleteTagCommand, DeleteTagCommandResult>
{
    private readonly ITagRepository _tagRepository;

    public DeleteTagCommandHandler(ITagRepository tagRepository)
    {
        _tagRepository = tagRepository;
    }

    public async Task<DeleteTagCommandResult> HandleAsync(DeleteTagCommand command, CancellationToken cancellationToken)
    {
        var tag = await _tagRepository.GetByIdAsync(command.TagId, cancellationToken);
        tag.Delete();
        await _tagRepository.DeleteAsync(tag, cancellationToken);
        return new DeleteTagCommandResult(tag.Id.Value);
    }
}
