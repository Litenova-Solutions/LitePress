using LiteBus.Commands.Abstractions;

namespace LiteNova.Blog.Application.Posts.PublishPost;

/// <summary>Command to publish a blog post.</summary>
public sealed record PublishPostCommand(Guid Id) : ICommand<PublishPostResult>;
