namespace LiteNova.Blog.Application.Read.Contracts.Shared;

public sealed record PaginationParameters
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 20;
    public bool SkipTotalCount { get; init; } = false;
    public const int MaxPageSize = 100;
}
