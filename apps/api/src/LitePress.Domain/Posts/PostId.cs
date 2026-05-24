using LitePress.Domain.Shared.StronglyTypedIds;

namespace LitePress.Domain.Posts;

public readonly record struct PostId(Guid Value) : IStronglyTypedId, IParsable<PostId>
{
    public static PostId New() => new(Guid.CreateVersion7());
    public static PostId Empty => new(Guid.Empty);

    public static PostId Parse(string s, IFormatProvider? provider) => new(Guid.Parse(s));

    public static bool TryParse(string? s, IFormatProvider? provider, out PostId result)
    {
        if (Guid.TryParse(s, out var guid))
        {
            result = new PostId(guid);
            return true;
        }
        result = default;
        return false;
    }

    public override string ToString() => Value.ToString();
}
