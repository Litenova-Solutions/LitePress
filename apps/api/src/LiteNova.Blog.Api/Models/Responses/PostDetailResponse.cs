namespace LiteNova.Blog.Api.Models.Responses;

/// <summary>
/// Response payload representing full post details.
/// </summary>
public sealed record PostDetailResponse : PostSummaryResponse
{
    /// <summary>
    /// TipTap JSON body content.
    /// </summary>
    public required string Body { get; init; }
}
