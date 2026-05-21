using LiteNova.Blog.Domain.Shared.StronglyTypedIds;

namespace LiteNova.Blog.Domain.Tags;

public readonly record struct TagId(Guid Value) : IStronglyTypedId, IParsable<TagId>
{
    public static TagId New() => new(Guid.CreateVersion7());
    public static TagId Empty => new(Guid.Empty);

    public static TagId Parse(string s, IFormatProvider? provider) => new(Guid.Parse(s));

    public static bool TryParse(string? s, IFormatProvider? provider, out TagId result)
    {
        if (Guid.TryParse(s, out var guid))
        {
            result = new TagId(guid);
            return true;
        }
        result = default;
        return false;
    }

    public override string ToString() => Value.ToString();
}
