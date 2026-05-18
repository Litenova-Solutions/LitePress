using Ardalis.GuardClauses;
using LiteBus.Queries.Abstractions;

namespace LiteNova.Blog.Application.Posts.GetPostBySlug;

public sealed class GetPostBySlugQueryValidator : IQueryValidator<GetPostBySlugQuery>
{
    public Task ValidateAsync(GetPostBySlugQuery query, CancellationToken cancellationToken)
    {
        Guard.Against.Null(query);
        return Task.CompletedTask;
    }
}
