using LiteNova.Blog.Application.Write.Contracts.Posts.PublishPost;

namespace LiteNova.Blog.Application.Write.Posts.Publish;

internal sealed class PublishPostCommandHandler : ICommandHandler<PublishPostCommand, PublishPostCommandResult>
{
    private readonly IPostRepository _postRepository;

    public PublishPostCommandHandler(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task<PublishPostCommandResult> HandleAsync(PublishPostCommand command, CancellationToken cancellationToken)
    {
        var post = await _postRepository.GetByIdAsync(command.PostId, cancellationToken);
        post.Publish();
        await _postRepository.UpdateAsync(post, cancellationToken);
        return new PublishPostCommandResult(post.Id.Value);
    }
}
