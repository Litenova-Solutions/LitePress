using LiteBus.Commands.Abstractions;
namespace LiteNova.Blog.Application.Posts.DeletePost;
public sealed record DeletePostCommand(Guid Id) : ICommand;
