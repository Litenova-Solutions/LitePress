namespace LitePress.Domain.Posts;

public sealed record PostExcerpt
{
    public string Value { get; }

    public PostExcerpt(string value)
    {
        if (value.Length > 500)
        {
            throw new ArgumentException("Post excerpt cannot exceed 500 characters.", nameof(value));
        }

        Value = value.Trim();
    }

    public override string ToString() => Value;
}
