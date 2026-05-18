using Ardalis.GuardClauses;
using LiteBus.Queries.Abstractions;

namespace LiteNova.Blog.Application.Posts.GetAllPosts;

public sealed class GetAllPostsQueryValidator : IQueryValidator<GetAllPostsQuery>
{
    public Task ValidateAsync(GetAllPostsQuery query, CancellationToken cancellationToken)
    {
        Guard.Against.Null(query);
        return Task.CompletedTask;
    }
}
