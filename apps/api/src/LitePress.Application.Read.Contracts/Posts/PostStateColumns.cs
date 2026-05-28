namespace LitePress.Application.Read.Contracts.Posts;

/// <summary>
/// Post lifecycle discriminator values used when projecting post state in read models.
/// </summary>
public static class PostStateColumns
{
    public const string StateType = "state_type";
    public const string PublishedAt = "published_at";
    public const string ArchivedAt = "archived_at";

    public const string Draft = "Draft";
    public const string Published = "Published";
    public const string Archived = "Archived";
}
