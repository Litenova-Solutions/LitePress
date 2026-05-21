using LiteNova.Blog.Application.Write.Contracts.Posts.ArchivePost;

namespace LiteNova.Blog.Application.Write.Posts.Archive;

internal sealed class ArchivePostCommandHandler : ICommandHandler<ArchivePostCommand, ArchivePostCommandResult>
{
    private readonly IPostRepository _postRepository;

    public ArchivePostCommandHandler(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task<ArchivePostCommandResult> HandleAsync(ArchivePostCommand command, CancellationToken cancellationToken)
    {
        var post = await _postRepository.GetByIdAsync(command.PostId, cancellationToken);
        post.Archive();
        await _postRepository.UpdateAsync(post, cancellationToken);
        return new ArchivePostCommandResult(post.Id.Value);
    }
}
