using LiteBus.CQRS;
using LiteNova.Blog.Application.Common.Interfaces;
using LiteNova.Blog.Domain.Posts;
using LiteNova.Blog.Domain.Tags;
using Microsoft.EntityFrameworkCore;

namespace LiteNova.Blog.Application.Posts.Commands.CreatePost;

public sealed class CreatePostCommandHandler(IBlogDbContext dbContext) : ICommandHandler<CreatePostCommand, CreatePostResult>
{
    public async Task<CreatePostResult> HandleAsync(CreatePostCommand command, CancellationToken cancellationToken)
    {
        var tags = await dbContext.Tags.Where(t => command.TagIds.Contains(t.Id)).ToListAsync(cancellationToken);
        var post = Post.Create(command.Title, command.Excerpt, command.Body, command.CoverImageUrl, tags);
        dbContext.Posts.Add(post);
        return new CreatePostResult(post.Id);
    }
}
