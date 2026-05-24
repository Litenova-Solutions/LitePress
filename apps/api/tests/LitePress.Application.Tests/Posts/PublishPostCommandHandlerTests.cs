using LitePress.Application.Write.Contracts.Posts.PublishPost;
using LitePress.Application.Write.Posts.Publish;
using LitePress.Domain.Authors;
using LitePress.Domain.Posts;
using NSubstitute;

namespace LitePress.Application.Tests.Posts;

public sealed class PublishPostCommandHandlerTests
{
    [Fact]
    public async Task HandleAsync_PublishesDraftPost()
    {
        var post = Post.Create(
            PostId.New(),
            new PostTitle("Title"),
            new PostContent("{\"type\":\"doc\",\"content\":[]}"),
            AuthorId.New());

        var repository = Substitute.For<IPostRepository>();
        repository.GetByIdAsync(Arg.Any<PostId>(), Arg.Any<CancellationToken>())
            .Returns(post);

        var handler = new PublishPostCommandHandler(repository);
        var command = new PublishPostCommand { PostId = post.Id };

        var result = await handler.HandleAsync(command, CancellationToken.None);

        result.PostId.Should().Be(post.Id.Value);
        post.State.Should().BeOfType<PublishedPostState>();
        await repository.Received(1).UpdateAsync(post, Arg.Any<CancellationToken>());
    }
}
