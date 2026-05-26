using LitePress.Application.Write.Contracts.Posts.ArchivePost;
using LitePress.Application.Write.Contracts.Shared;

namespace LitePress.Application.Write.Posts.Archive;

internal sealed class ArchivePostCommandHandler : ICommandHandler<ArchivePostCommand, ArchivePostCommandResult>
{
    private readonly IPostRepository _postRepository;
    private readonly IClock _clock;

    public ArchivePostCommandHandler(IPostRepository postRepository, IClock clock)
    {
        _postRepository = postRepository;
        _clock = clock;
    }

    public async Task<ArchivePostCommandResult> HandleAsync(ArchivePostCommand command, CancellationToken cancellationToken)
    {
        var post = await _postRepository.GetByIdAsync(command.PostId, cancellationToken);
        post.Archive(_clock.UtcNow);
        await _postRepository.UpdateAsync(post, cancellationToken);
        return new ArchivePostCommandResult(post.Id.Value);
    }
}
