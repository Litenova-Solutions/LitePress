using LiteNova.Blog.Application.Write.Contracts.Posts.UpdatePost;
using LiteNova.Blog.Application.Write.Contracts.Posts.CreatePost.Exceptions;

namespace LiteNova.Blog.Application.Write.Posts.Update;

internal sealed class UpdatePostCommandHandler : ICommandHandler<UpdatePostCommand, UpdatePostCommandResult>
{
    private readonly IPostRepository _postRepository;

    public UpdatePostCommandHandler(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task<UpdatePostCommandResult> HandleAsync(UpdatePostCommand command, CancellationToken cancellationToken)
    {
        var post = await _postRepository.GetByIdAsync(command.PostId, cancellationToken);

        post.Update(
            new PostTitle(command.Title),
            new PostContent(command.Content),
            command.Excerpt is not null ? new PostExcerpt(command.Excerpt) : null,
            command.CoverImageUrl is not null ? new PostCoverImageUrl(command.CoverImageUrl) : null);

        await _postRepository.UpdateAsync(post, cancellationToken);

        return new UpdatePostCommandResult(post.Id.Value, post.Slug.Value);
    }
}
