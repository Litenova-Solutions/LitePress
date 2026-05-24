using LitePress.Application.Read.Contracts.Shared;

namespace LitePress.Application.Read.Contracts.Posts.GetPostById;

public sealed record GetPostByIdQuery : IQuery<PostDetailResult>
{
    public required PostId PostId { get; init; }
}
