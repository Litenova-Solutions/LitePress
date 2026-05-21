using LiteNova.Blog.Application.Write.Contracts.Posts.UpdatePost;
using LiteNova.Blog.Application.Write.Contracts.Posts.CreatePost.Exceptions;

namespace LiteNova.Blog.Application.Write.Posts.Update;

internal sealed class UpdatePostCommandValidator : ICommandValidator<UpdatePostCommand>
{
    public Task ValidateAsync(UpdatePostCommand command, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(command.Title))
        {
            throw new PostTitleRequiredException();
        }

        if (command.Title.Length > 200)
        {
            throw new PostTitleTooLongException(command.Title.Length);
        }

        if (string.IsNullOrWhiteSpace(command.Content))
        {
            throw new PostContentRequiredException();
        }

        if (command.Excerpt is not null && command.Excerpt.Length > 500)
        {
            throw new PostExcerptTooLongException(command.Excerpt.Length);
        }

        return Task.CompletedTask;
    }
}
