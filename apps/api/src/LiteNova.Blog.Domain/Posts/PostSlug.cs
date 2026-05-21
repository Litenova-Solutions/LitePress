using System.Text.RegularExpressions;

namespace LiteNova.Blog.Domain.Posts;

public sealed record PostSlug
{
    private static readonly Regex SlugPattern = new(@"^[a-z0-9]+(?:-[a-z0-9]+)*$", RegexOptions.Compiled);
    private static readonly Regex NonAlphanumericPattern = new(@"[^a-z0-9\s-]", RegexOptions.Compiled);
    private static readonly Regex WhitespacePattern = new(@"\s+", RegexOptions.Compiled);

    public string Value { get; }

    public PostSlug(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Post slug cannot be empty.", nameof(value));
        }

        if (!SlugPattern.IsMatch(value))
        {
            throw new ArgumentException("Post slug must be URL-safe lowercase hyphen-separated.", nameof(value));
        }

        Value = value;
    }

    public static PostSlug FromTitle(string title)
    {
        var normalized = title.ToLowerInvariant().Trim();
        normalized = NonAlphanumericPattern.Replace(normalized, string.Empty);
        normalized = WhitespacePattern.Replace(normalized, "-").Trim('-');

        if (string.IsNullOrWhiteSpace(normalized))
        {
            normalized = "post";
        }

        return new PostSlug(normalized);
    }

    public override string ToString() => Value;
}
