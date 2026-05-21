using System.Text.RegularExpressions;

namespace LiteNova.Blog.Domain.Tags;

public sealed record TagSlug
{
    private static readonly Regex SlugPattern = new(@"^[a-z0-9]+(?:-[a-z0-9]+)*$", RegexOptions.Compiled);
    private static readonly Regex NonAlphanumericPattern = new(@"[^a-z0-9\s-]", RegexOptions.Compiled);
    private static readonly Regex WhitespacePattern = new(@"\s+", RegexOptions.Compiled);

    public string Value { get; }

    public TagSlug(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Tag slug cannot be empty.", nameof(value));
        }

        if (!SlugPattern.IsMatch(value))
        {
            throw new ArgumentException("Tag slug must be URL-safe lowercase hyphen-separated.", nameof(value));
        }

        Value = value;
    }

    public static TagSlug FromName(string name)
    {
        var normalized = name.ToLowerInvariant().Trim();
        normalized = NonAlphanumericPattern.Replace(normalized, string.Empty);
        normalized = WhitespacePattern.Replace(normalized, "-").Trim('-');
        if (string.IsNullOrWhiteSpace(normalized))
        {
            normalized = "tag";
        }

        return new TagSlug(normalized);
    }

    public override string ToString() => Value;
}
