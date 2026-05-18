using LiteBus.Commands.Abstractions;

namespace LiteNova.Blog.Application.Posts.DeletePost;

/// <summary>Command to delete a blog post.</summary>
public sealed record DeletePostCommand(Guid Id) : ICommand;
