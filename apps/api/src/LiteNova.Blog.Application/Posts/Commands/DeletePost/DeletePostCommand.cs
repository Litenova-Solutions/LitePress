using LiteBus.CQRS;
namespace LiteNova.Blog.Application.Posts.Commands.DeletePost;
public sealed record DeletePostCommand(Guid Id) : ICommand;
