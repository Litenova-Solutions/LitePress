using LiteNova.Blog.Domain.Shared.Exceptions;
using LiteNova.Blog.Domain.Tags;

namespace LiteNova.Blog.Domain.Posts.Exceptions;

public sealed class PostTagAlreadyAssignedException : DomainException
{
    public PostTagAlreadyAssignedException(PostId postId, TagId tagId)
        : base($"Tag ''{tagId.Value}'' is already assigned to post ''{postId.Value}''.") { }
}
