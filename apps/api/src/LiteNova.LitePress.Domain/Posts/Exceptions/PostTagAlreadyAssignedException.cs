using LiteNova.LitePress.Domain.Shared.Exceptions;
using LiteNova.LitePress.Domain.Tags;

namespace LiteNova.LitePress.Domain.Posts.Exceptions;

public sealed class PostTagAlreadyAssignedException : DomainException
{
    public PostTagAlreadyAssignedException(PostId postId, TagId tagId)
        : base($"Tag ''{tagId.Value}'' is already assigned to post ''{postId.Value}''.") { }
}
