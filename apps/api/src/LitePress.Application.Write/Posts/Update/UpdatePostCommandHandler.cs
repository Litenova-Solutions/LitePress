using LitePress.Application.Write.Contracts.Posts.UpdatePost;
using LitePress.Application.Write.Contracts.Shared;

namespace LitePress.Application.Write.Posts.Update;

internal sealed class UpdatePostCommandHandler : ICommandHandler<UpdatePostCommand, UpdatePostCommandResult>
{
    private readonly IPostRepository _postRepository;
    private readonly IClock _clock;

    public UpdatePostCommandHandler(IPostRepository postRepository, IClock clock)
    {
        _postRepository = postRepository;
        _clock = clock;
    }

    public async Task<UpdatePostCommandResult> HandleAsync(UpdatePostCommand command, CancellationToken cancellationToken)
    {
        var post = await _postRepository.GetByIdAsync(command.PostId, cancellationToken);

        post.Update(
            new PostTitle(command.Title),
            new PostContent(command.Content),
            command.Excerpt is not null ? new PostExcerpt(command.Excerpt) : null,
            command.CoverImageUrl is not null ? new PostCoverImageUrl(command.CoverImageUrl) : null,
            _clock.UtcNow);

        await _postRepository.UpdateAsync(post, cancellationToken);

        return new UpdatePostCommandResult(post.Id.Value, post.Slug.Value);
    }
}
