namespace LitePress.Application.Read.Posts;

internal static class PostStateQuery
{
    internal static DateTimeOffset? GetPublishedAt(PostState state) =>
        state is PublishedPostState published ? published.PublishedAt : null;
}
