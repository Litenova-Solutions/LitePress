using LitePress.Domain.Shared.Exceptions;
using LitePress.Domain.Tags;

namespace LitePress.Domain.Posts.Exceptions;

public sealed class PostTagNotAssignedException : DomainException
{
    public PostTagNotAssignedException(PostId postId, TagId tagId)
        : base($"Tag ''{tagId.Value}'' is not assigned to post ''{postId.Value}''.") { }
}
