using LiteBus.CQRS;
namespace LiteNova.Blog.Application.Posts.Commands.PublishPost;
public sealed record PublishPostCommand(Guid Id) : ICommand<PublishPostResult>;
