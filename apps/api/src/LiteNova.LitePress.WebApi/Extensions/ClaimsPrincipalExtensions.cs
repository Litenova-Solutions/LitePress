using LiteNova.LitePress.Domain.Authors;

namespace LiteNova.LitePress.WebApi.Extensions;

internal static class ClaimsPrincipalExtensions
{
    private const string AuthorIdClaimType = "author_id";

    internal static AuthorId GetAuthorId(this ClaimsPrincipal user)
    {
        var claim = user.FindFirst(AuthorIdClaimType)
            ?? throw new InvalidOperationException("Author ID claim not found.");

        return new AuthorId(Guid.Parse(claim.Value));
    }
}
