using LiteNova.Blog.Domain.Common;

namespace LiteNova.Blog.Domain.Tags;

/// <summary>The Tag aggregate root representing a categorization label for blog posts.</summary>
public class Tag : AggregateRoot
{
    public string Name { get; private set; } = string.Empty;
    public string Slug { get; private set; } = string.Empty;

    private Tag() { }

    public static Tag Create(string name)
    {
        return new Tag
        {
            Name = name.Trim(),
            Slug = string.Join('-', name.ToLowerInvariant().Split(' ', StringSplitOptions.RemoveEmptyEntries))
        };
    }
}
