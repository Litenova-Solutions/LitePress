using LiteNova.Blog.Domain.Authors;
using LiteNova.Blog.Domain.Posts;
using LiteNova.Blog.Domain.Posts.Exceptions;
using LiteNova.Blog.Domain.Tags;

namespace LiteNova.Blog.Domain.Tests.Posts;

public sealed class PostTests
{
    [Fact]
    public void Publish_FromDraft_SetsPublishedState()
    {
        var post = CreateDraftPost();

        post.Publish();

        post.State.Should().BeOfType<PublishedPostState>();
        post.PublishedAt.Should().NotBeNull();
    }

    [Fact]
    public void Publish_WhenAlreadyPublished_ThrowsPostAlreadyPublishedException()
    {
        var post = CreateDraftPost();
        post.Publish();

        var act = () => post.Publish();

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
            AuthorId.New());
    }
}
