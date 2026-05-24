namespace LitePress.Domain.Posts;

public sealed record PostCoverImageUrl
{
    public string Value { get; }

    public PostCoverImageUrl(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Cover image URL cannot be empty.", nameof(value));
        }

        if (!Uri.TryCreate(value, UriKind.Absolute, out _))
        {
            throw new ArgumentException("Cover image URL must be a valid absolute URL.", nameof(value));
        }

        Value = value;
    }

    public override string ToString() => Value;
}
