using LiteBus.Commands.Abstractions;
namespace LiteNova.Blog.Application.Posts.PublishPost;
public sealed record PublishPostCommand(Guid Id) : ICommand<PublishPostResult>;
