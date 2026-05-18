using LiteBus.Commands.Abstractions;
using LiteNova.Blog.Application.Common.Exceptions;
using LiteNova.Blog.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LiteNova.Blog.Application.Posts.SchedulePost;

/// <summary>
/// Handles the SchedulePostCommand use case.
/// </summary>
public sealed class SchedulePostCommandHandler(IBlogDbContext dbContext) : ICommandHandler<SchedulePostCommand, SchedulePostResult>
{
    public async Task<SchedulePostResult> HandleAsync(SchedulePostCommand command, CancellationToken cancellationToken)
    {
        var post = await dbContext.Posts.FirstOrDefaultAsync(p => p.Id == command.Id, cancellationToken) ?? throw new PostNotFoundException(command.Id);
        post.Schedule(command.ScheduledFor);
        return new SchedulePostResult(post.Id);
    }
}
