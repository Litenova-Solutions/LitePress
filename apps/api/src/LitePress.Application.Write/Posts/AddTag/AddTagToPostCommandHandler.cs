using LitePress.Application.Write.Contracts.Posts.AddTagToPost;

namespace LitePress.Application.Write.Posts.AddTag;

internal sealed class AddTagToPostCommandHandler : ICommandHandler<AddTagToPostCommand, AddTagToPostCommandResult>
{
    private readonly IPostRepository _postRepository;

    public AddTagToPostCommandHandler(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task<AddTagToPostCommandResult> HandleAsync(AddTagToPostCommand command, CancellationToken cancellationToken)
    {
        var post = await _postRepository.GetByIdAsync(command.PostId, cancellationToken);
        post.AddTag(command.TagId);
        await _postRepository.UpdateAsync(post, cancellationToken);
        return new AddTagToPostCommandResult(post.Id.Value, command.TagId.Value);
    }
}
