using LiteNova.Blog.Domain.Shared.StronglyTypedIds;

namespace LiteNova.Blog.Domain.Authors;

public readonly record struct AuthorId(Guid Value) : IStronglyTypedId, IParsable<AuthorId>
{
    public static AuthorId New() => new(Guid.CreateVersion7());
    public static AuthorId Empty => new(Guid.Empty);

    public static AuthorId Parse(string s, IFormatProvider? provider) => new(Guid.Parse(s));

    public static bool TryParse(string? s, IFormatProvider? provider, out AuthorId result)
    {
        if (Guid.TryParse(s, out var guid))
        {
            result = new AuthorId(guid);
            return true;
        }
        result = default;
        return false;
    }

    public override string ToString() => Value.ToString();
}
