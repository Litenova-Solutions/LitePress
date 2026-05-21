using LiteNova.Blog.Application.Read.Contracts.Authors.GetAuthorById;

namespace LiteNova.Blog.Application.Read.Contracts.Authors.GetAuthorById;

public sealed record GetAuthorByIdQuery : IQuery<AuthorResult>
{
    public required AuthorId AuthorId { get; init; }
}
