using LiteBus.CQRS;
namespace LiteNova.Blog.Application.Posts.Queries.GetPostBySlug;
public sealed record GetPostBySlugQuery(string Slug) : IQuery<GetPostBySlugQueryResult>;
