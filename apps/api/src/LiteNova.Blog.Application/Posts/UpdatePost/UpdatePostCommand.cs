using LiteBus.Commands.Abstractions;

namespace LiteNova.Blog.Application.Posts.UpdatePost;

/// <summary>Command to update an existing blog post.</summary>
public sealed record UpdatePostCommand(Guid Id, string Title, string Excerpt, string Body, string? CoverImageUrl, IReadOnlyCollection<Guid> TagIds) : ICommand<UpdatePostResult>;
