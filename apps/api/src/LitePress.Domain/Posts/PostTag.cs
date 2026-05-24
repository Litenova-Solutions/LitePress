using LitePress.Domain.Tags;

namespace LitePress.Domain.Posts;

public sealed class PostTag
{
    private PostTag() { }

    public PostTag(PostId postId, TagId tagId)
    {
        PostId = postId;
        TagId = tagId;
    }

    public PostId PostId { get; private set; }
    public TagId TagId { get; private set; }
}
