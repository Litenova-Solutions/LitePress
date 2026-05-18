using LiteNova.Blog.Domain.Common;
namespace LiteNova.Blog.Domain.Posts.Exceptions;
public sealed class PostAlreadyScheduledException(Guid postId) : DomainException($"Post {postId} is already scheduled.");
