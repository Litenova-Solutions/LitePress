using LitePress.Application.Write.Contracts.Posts.PublishPost;
using LitePress.Application.Write.Contracts.Shared;
using LitePress.Application.Write.Posts.Publish;
using LitePress.Domain.Authors;
using LitePress.Domain.Posts;
using NSubstitute;

namespace LitePress.Application.Tests.Posts;

public sealed class PublishPostCommandHandlerTests
{
    private static readonly DateTimeOffset TestNow =
        new(2026, 5, 26, 12, 0, 0, TimeSpan.Zero);

    [Fact]
    public async Task HandleAsync_PublishesDraftPost()
    {
        var post = Post.Create(
            PostId.New(),
            new PostTitle("Title"),
            new PostContent("{\"type\":\"doc\",\"content\":[]}"),
            AuthorId.New(),
            TestNow);

        var repository = Substitute.For<IPostRepository>();
        repository.GetByIdAsync(Arg.Any<PostId>(), Arg.Any<CancellationToken>())
            .Returns(post);

        var clock = Substitute.For<IClock>();
        clock.UtcNow.Returns(TestNow);

        var handler = new PublishPostCommandHandler(repository, clock);
        var command = new PublishPostCommand { PostId = post.Id };

        var result = await handler.HandleAsync(command, CancellationToken.None);

        result.PostId.Should().Be(post.Id.Value);
        post.State.Should().BeOfType<PublishedPostState>();
        await repository.Received(1).UpdateAsync(post, Arg.Any<CancellationToken>());
    }
}
