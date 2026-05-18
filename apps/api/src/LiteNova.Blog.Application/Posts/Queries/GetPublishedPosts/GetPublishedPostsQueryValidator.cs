using Ardalis.GuardClauses;
using LiteBus.CQRS;

namespace LiteNova.Blog.Application.Posts.Queries.GetPublishedPosts;

public sealed class GetPublishedPostsQueryValidator : IQueryValidator<GetPublishedPostsQuery>
{
    public Task ValidateAsync(GetPublishedPostsQuery query, CancellationToken cancellationToken)
    {
        Guard.Against.Null(query);
        return Task.CompletedTask;
    }
}
