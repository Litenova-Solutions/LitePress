using LiteBus.CQRS;
namespace LiteNova.Blog.Application.Posts.Commands.CreatePost;
public sealed record CreatePostCommand(string Title, string Excerpt, string Body, string? CoverImageUrl, IReadOnlyCollection<Guid> TagIds) : ICommand<CreatePostResult>;
