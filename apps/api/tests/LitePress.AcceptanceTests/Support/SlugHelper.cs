using LitePress.Domain.Posts;
using LitePress.Domain.Tags;

namespace LitePress.AcceptanceTests.Support;

/// <summary>
/// Derives expected URL slugs using the same domain rules as the API, for Then-step assertions.
/// </summary>
internal static class SlugHelper
{
    /// <summary>Slug the API would assign from a post title.</summary>
    internal static string PostSlugFromTitle(string title) =>
        PostSlug.FromTitle(title).Value;

    /// <summary>Slug the API would assign from a tag name.</summary>
    internal static string TagSlugFromName(string name) =>
        TagSlug.FromName(name).Value;
}
