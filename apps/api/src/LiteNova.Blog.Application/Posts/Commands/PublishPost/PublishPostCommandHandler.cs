using LiteBus.CQRS;
using LiteNova.Blog.Application.Common.Exceptions;
using LiteNova.Blog.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LiteNova.Blog.Application.Posts.Commands.PublishPost;

public sealed class PublishPostCommandHandler(IBlogDbContext dbContext) : ICommandHandler<PublishPostCommand, PublishPostResult>
{
    public async Task<PublishPostResult> HandleAsync(PublishPostCommand command, CancellationToken cancellationToken)
    {
        var post = await dbContext.Posts.FirstOrDefaultAsync(p => p.Id == command.Id, cancellationToken) ?? throw new PostNotFoundException(command.Id);
        post.Publish();
        return new PublishPostResult(post.Id);
    }
}
