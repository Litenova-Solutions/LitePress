using Ardalis.GuardClauses;
using LiteBus.CQRS;

namespace LiteNova.Blog.Application.Posts.Queries.GetAllPosts;

public sealed class GetAllPostsQueryValidator : IQueryValidator<GetAllPostsQuery>
{
    public Task ValidateAsync(GetAllPostsQuery query, CancellationToken cancellationToken)
    {
        Guard.Against.Null(query);
        return Task.CompletedTask;
    }
}
