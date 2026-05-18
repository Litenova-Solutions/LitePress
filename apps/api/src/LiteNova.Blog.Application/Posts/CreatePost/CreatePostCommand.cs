using LiteBus.Commands.Abstractions;
namespace LiteNova.Blog.Application.Posts.CreatePost;
public sealed record CreatePostCommand(string Title, string Excerpt, string Body, string? CoverImageUrl, IReadOnlyCollection<Guid> TagIds) : ICommand<CreatePostResult>;
