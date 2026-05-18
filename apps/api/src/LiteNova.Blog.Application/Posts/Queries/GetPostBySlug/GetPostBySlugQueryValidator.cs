using Ardalis.GuardClauses;
using LiteBus.CQRS;

namespace LiteNova.Blog.Application.Posts.Queries.GetPostBySlug;

public sealed class GetPostBySlugQueryValidator : IQueryValidator<GetPostBySlugQuery>
{
    public Task ValidateAsync(GetPostBySlugQuery query, CancellationToken cancellationToken)
    {
        Guard.Against.Null(query);
        return Task.CompletedTask;
    }
}
