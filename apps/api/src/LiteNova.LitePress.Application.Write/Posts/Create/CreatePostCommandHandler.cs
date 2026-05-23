using LiteNova.LitePress.Application.Write.Contracts.Posts.CreatePost;
using LiteNova.LitePress.Application.Write.Contracts.Posts.CreatePost.Exceptions;
using LiteNova.LitePress.Domain.Posts.Exceptions;

namespace LiteNova.LitePress.Application.Write.Posts.Create;

internal sealed class CreatePostCommandHandler : ICommandHandler<CreatePostCommand, CreatePostCommandResult>
{
    private readonly IPostRepository _postRepository;

    public CreatePostCommandHandler(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task<CreatePostCommandResult> HandleAsync(CreatePostCommand command, CancellationToken cancellationToken)
    {
        var slug = PostSlug.FromTitle(command.Title);

        if (await _postRepository.SlugExistsAsync(slug, cancellationToken))
        {
            throw new PostSlugAlreadyExistsException(slug);
        }

        var tagIds = command.TagIds.Select(id => new TagId(id)).ToList();

        var post = Post.Create(
            command.PostId,
            new PostTitle(command.Title),
            new PostContent(command.Content),
            command.AuthorId,
            command.Excerpt is not null ? new PostExcerpt(command.Excerpt) : null,
            command.CoverImageUrl is not null ? new PostCoverImageUrl(command.CoverImageUrl) : null,
            tagIds);

        await _postRepository.AddAsync(post, cancellationToken);

        return new CreatePostCommandResult(post.Id.Value, post.Slug.Value);
    }
}
