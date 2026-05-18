using LiteBus.Queries.Abstractions;
namespace LiteNova.Blog.Application.Posts.GetPostBySlug;
public sealed record GetPostBySlugQuery(string Slug) : IQuery<GetPostBySlugQueryResult>;
