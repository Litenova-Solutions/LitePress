namespace LiteNova.Blog.Domain.Tags;

public sealed record TagName
{
    public string Value { get; }

    public TagName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Tag name cannot be empty.", nameof(value));
        }

        if (value.Length > 50)
        {
            throw new ArgumentException("Tag name cannot exceed 50 characters.", nameof(value));
        }

        Value = value.Trim();
    }

    public override string ToString() => Value;
}
