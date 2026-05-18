using LiteBus.Commands.Abstractions;
using LiteNova.Blog.Application.Common.Exceptions;
using LiteNova.Blog.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LiteNova.Blog.Application.Posts.DeletePost;

/// <summary>
/// Handles the DeletePostCommand use case.
/// </summary>
public sealed class DeletePostCommandHandler(IBlogDbContext dbContext) : ICommandHandler<DeletePostCommand>
{
    public async Task HandleAsync(DeletePostCommand command, CancellationToken cancellationToken)
    {
        var post = await dbContext.Posts.FirstOrDefaultAsync(p => p.Id == command.Id, cancellationToken) ?? throw new PostNotFoundException(command.Id);
        post.Delete();
        dbContext.Posts.Remove(post);
    }
}
