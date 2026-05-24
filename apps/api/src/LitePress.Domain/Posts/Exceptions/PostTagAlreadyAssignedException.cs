using LitePress.Domain.Shared.Exceptions;
using LitePress.Domain.Tags;

namespace LitePress.Domain.Posts.Exceptions;

public sealed class PostTagAlreadyAssignedException : DomainException
{
    public PostTagAlreadyAssignedException(PostId postId, TagId tagId)
        : base($"Tag ''{tagId.Value}'' is already assigned to post ''{postId.Value}''.") { }
}
