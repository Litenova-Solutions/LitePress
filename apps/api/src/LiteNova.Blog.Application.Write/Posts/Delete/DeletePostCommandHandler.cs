using LiteNova.Blog.Application.Write.Contracts.Posts.DeletePost;

namespace LiteNova.Blog.Application.Write.Posts.Delete;

internal sealed class DeletePostCommandHandler : ICommandHandler<DeletePostCommand, DeletePostCommandResult>
{
    private readonly IPostRepository _postRepository;

    public DeletePostCommandHandler(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task<DeletePostCommandResult> HandleAsync(DeletePostCommand command, CancellationToken cancellationToken)
    {
        var post = await _postRepository.GetByIdAsync(command.PostId, cancellationToken);
        post.Delete();
        await _postRepository.DeleteAsync(post, cancellationToken);
        return new DeletePostCommandResult(post.Id.Value);
    }
}
