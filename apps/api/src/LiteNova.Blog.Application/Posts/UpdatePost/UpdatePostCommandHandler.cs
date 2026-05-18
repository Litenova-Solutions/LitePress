using LiteBus.Commands.Abstractions;
using LiteNova.Blog.Application.Common.Exceptions;
using LiteNova.Blog.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LiteNova.Blog.Application.Posts.UpdatePost;

/// <summary>Handles the <see cref="UpdatePostCommand"/> by updating an existing blog post.</summary>
public sealed class UpdatePostCommandHandler(IBlogDbContext dbContext) : ICommandHandler<UpdatePostCommand, UpdatePostResult>
{
    public async Task<UpdatePostResult> HandleAsync(UpdatePostCommand command, CancellationToken cancellationToken)
    {
        var post = await dbContext.Posts.FirstOrDefaultAsync(p => p.Id == command.Id, cancellationToken) ?? throw new PostNotFoundException(command.Id);
        var tags = await dbContext.Tags.Where(t => command.TagIds.Contains(t.Id)).ToListAsync(cancellationToken);
        post.Update(command.Title, command.Excerpt, command.Body, command.CoverImageUrl, tags);
        return new UpdatePostResult(post.Id);
    }
}
