using LiteBus.Commands.Abstractions;
namespace LiteNova.Blog.Application.Posts.UpdatePost;
public sealed record UpdatePostCommand(Guid Id, string Title, string Excerpt, string Body, string? CoverImageUrl, IReadOnlyCollection<Guid> TagIds) : ICommand<UpdatePostResult>;
