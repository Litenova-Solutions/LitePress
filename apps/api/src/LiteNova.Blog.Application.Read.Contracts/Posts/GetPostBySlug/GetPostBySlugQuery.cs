using LiteNova.Blog.Application.Read.Contracts.Posts.GetPostById;

namespace LiteNova.Blog.Application.Read.Contracts.Posts.GetPostBySlug;

public sealed record GetPostBySlugQuery : IQuery<PostDetailResult>
{
    public required string Slug { get; init; }
}
