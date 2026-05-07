using LiteBus.CQRS;
namespace LiteNova.Blog.Application.Posts.Commands.UpdatePost;
public sealed record UpdatePostCommand(Guid Id, string Title, string Excerpt, string Body, string? CoverImageUrl, IReadOnlyCollection<Guid> TagIds) : ICommand<UpdatePostResult>;
