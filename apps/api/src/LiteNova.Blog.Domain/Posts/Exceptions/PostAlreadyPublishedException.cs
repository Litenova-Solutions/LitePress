using LiteNova.Blog.Domain.Common;
namespace LiteNova.Blog.Domain.Posts.Exceptions;
public sealed class PostAlreadyPublishedException(Guid postId) : DomainException($"Post {postId} is already published.");
