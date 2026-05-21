using LiteNova.Blog.Application.Read.Contracts.Shared;

namespace LiteNova.Blog.Application.Read.Contracts.Posts.GetPostById;

public sealed record GetPostByIdQuery : IQuery<PostDetailResult>
{
    public required PostId PostId { get; init; }
}
