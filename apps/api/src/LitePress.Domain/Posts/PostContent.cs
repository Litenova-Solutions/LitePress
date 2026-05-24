namespace LitePress.Domain.Posts;

public sealed record PostContent
{
    public string Value { get; }

    public PostContent(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Post content cannot be empty.", nameof(value));
        }

        Value = value;
    }

    public override string ToString() => Value;
}
