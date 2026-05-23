using LiteNova.LitePress.Domain.Shared.Exceptions;
using LiteNova.LitePress.Domain.Tags;

namespace LiteNova.LitePress.Domain.Posts.Exceptions;

public sealed class PostTagNotAssignedException : DomainException
{
    public PostTagNotAssignedException(PostId postId, TagId tagId)
        : base($"Tag ''{tagId.Value}'' is not assigned to post ''{postId.Value}''.") { }
}
