using LiteBus.Queries.Abstractions;

namespace LiteNova.Blog.Application.Posts.GetPostBySlug;

/// <summary>Query to retrieve a blog post by its slug.</summary>
public sealed record GetPostBySlugQuery(string Slug) : IQuery<GetPostBySlugQueryResult>;
