using LiteNova.LitePress.Application.Write.Contracts.Posts.RemoveTagFromPost;

namespace LiteNova.LitePress.Application.Write.Posts.RemoveTag;

internal sealed class RemoveTagFromPostCommandHandler : ICommandHandler<RemoveTagFromPostCommand, RemoveTagFromPostCommandResult>
{
    private readonly IPostRepository _postRepository;

    public RemoveTagFromPostCommandHandler(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task<RemoveTagFromPostCommandResult> HandleAsync(RemoveTagFromPostCommand command, CancellationToken cancellationToken)
    {
        var post = await _postRepository.GetByIdAsync(command.PostId, cancellationToken);
        post.RemoveTag(command.TagId);
        await _postRepository.UpdateAsync(post, cancellationToken);
        return new RemoveTagFromPostCommandResult(post.Id.Value, command.TagId.Value);
    }
}
