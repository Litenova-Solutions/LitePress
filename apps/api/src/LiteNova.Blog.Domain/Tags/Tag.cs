using LiteNova.Blog.Domain.Common;

namespace LiteNova.Blog.Domain.Tags;

/// <summary>
/// Tag aggregate root representing a reusable post label.
/// </summary>
public class Tag : AggregateRoot
{
    public string Name { get; private set; } = string.Empty;
    public string Slug { get; private set; } = string.Empty;

    private Tag()
    {
    }

    public static Tag Create(string name)
    {
        return new Tag
        {
            Name = name.Trim(),
            Slug = string.Join('-', name.ToLowerInvariant().Split(' ', StringSplitOptions.RemoveEmptyEntries))
        };
    }
}
