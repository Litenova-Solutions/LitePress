using LitePress.Application.Write.Contracts.Posts.PublishPost;
using LitePress.Application.Write.Contracts.Shared;

namespace LitePress.Application.Write.Posts.Publish;

internal sealed class PublishPostCommandHandler : ICommandHandler<PublishPostCommand, PublishPostCommandResult>
{
    private readonly IPostRepository _postRepository;
    private readonly IClock _clock;

    public PublishPostCommandHandler(IPostRepository postRepository, IClock clock)
    {
        _postRepository = postRepository;
        _clock = clock;
    }

    public async Task<PublishPostCommandResult> HandleAsync(PublishPostCommand command, CancellationToken cancellationToken)
    {
        var post = await _postRepository.GetByIdAsync(command.PostId, cancellationToken);
        post.Publish(_clock.UtcNow);
        await _postRepository.UpdateAsync(post, cancellationToken);
        return new PublishPostCommandResult(post.Id.Value);
    }
}
