using LiteNova.Blog.Domain.Shared.Exceptions;
using LiteNova.Blog.Domain.Tags;

namespace LiteNova.Blog.Domain.Posts.Exceptions;

public sealed class PostTagNotAssignedException : DomainException
{
    public PostTagNotAssignedException(PostId postId, TagId tagId)
        : base($"Tag ''{tagId.Value}'' is not assigned to post ''{postId.Value}''.") { }
}
