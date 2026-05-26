using LitePress.Domain.Authors;
using LitePress.Domain.Posts;
using LitePress.Domain.Posts.Events;
using LitePress.Domain.Posts.Exceptions;
using LitePress.Domain.Tags;

namespace LitePress.Domain.Tests.Posts;

public sealed class PostTests
{
    private static readonly DateTimeOffset TestNow =
        new(2026, 5, 26, 12, 0, 0, TimeSpan.Zero);

    [Fact]
    public void Publish_FromDraft_SetsPublishedState()
    {
        var post = CreateDraftPost();

        post.Publish(TestNow);

        post.State.Should().BeOfType<PublishedPostState>()
            .Which.PublishedAt.Should().Be(TestNow);
        post.DomainEvents.OfType<PostPublished>().Should().ContainSingle();
    }

    [Fact]
    public void Publish_WhenAlreadyPublished_ThrowsPostAlreadyPublishedException()
    {
        var post = CreateDraftPost();
        post.Publish(TestNow);

        var act = () => post.Publish(TestNow);

        act.Should().Throw<PostAlreadyPublishedException>();
    }

    [Fact]
    public void AddTag_WhenDuplicate_ThrowsPostTagAlreadyAssignedException()
    {
        var post = CreateDraftPost();
        var tagId = TagId.New();

        post.AddTag(tagId);

        var act = () => post.AddTag(tagId);

        act.Should().Throw<PostTagAlreadyAssignedException>();
    }

    private static Post CreateDraftPost()
    {
        return Post.Create(
            PostId.New(),
            new PostTitle("Hello World"),
            new PostContent("{\"type\":\"doc\",\"content\":[]}"),
            AuthorId.New(),
            TestNow);
    }
}
