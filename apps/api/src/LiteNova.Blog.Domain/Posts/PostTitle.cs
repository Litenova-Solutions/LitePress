namespace LiteNova.Blog.Domain.Posts;

public sealed record PostTitle
{
    public string Value { get; }

    public PostTitle(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Post title cannot be empty.", nameof(value));
        }

        if (value.Length > 200)
        {
            throw new ArgumentException("Post title cannot exceed 200 characters.", nameof(value));
        }

        Value = value.Trim();
    }

    public override string ToString() => Value;
}
