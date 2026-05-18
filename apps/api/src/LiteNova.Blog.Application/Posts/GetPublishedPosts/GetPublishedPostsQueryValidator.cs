using Ardalis.GuardClauses;
using LiteBus.Queries.Abstractions;

namespace LiteNova.Blog.Application.Posts.GetPublishedPosts;

public sealed class GetPublishedPostsQueryValidator : IQueryValidator<GetPublishedPostsQuery>
{
    public Task ValidateAsync(GetPublishedPostsQuery query, CancellationToken cancellationToken)
    {
        Guard.Against.Null(query);
        return Task.CompletedTask;
    }
}
