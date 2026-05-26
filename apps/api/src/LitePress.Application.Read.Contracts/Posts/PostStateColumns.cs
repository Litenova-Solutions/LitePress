namespace LitePress.Application.Read.Contracts.Posts;

/// <summary>
/// Shadow property and discriminator names for Post.State TPH column mapping.
/// Must stay aligned with <c>PostConfiguration</c> in Infrastructure.
/// </summary>
public static class PostStateColumns
{
    public const string StateType = "StateType";
    public const string PublishedAt = "PublishedAt";
    public const string ArchivedAt = "ArchivedAt";

    public const string Draft = "Draft";
    public const string Published = "Published";
    public const string Archived = "Archived";
}
