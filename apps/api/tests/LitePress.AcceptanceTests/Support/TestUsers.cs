using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace LitePress.AcceptanceTests.Support;

/// <summary>
/// JWT helpers for acceptance tests. Must match <c>JwtSettings:Secret</c> injected by
/// <see cref="AcceptanceTestWebAppFactory"/> so the API accepts scenario tokens.
/// </summary>
internal static class TestUsers
{
    /// <summary>Shared secret configured on the test host (minimum 32 characters).</summary>
    internal const string DevJwtSecret = "dev-secret-key-must-be-at-least-32-characters-long!";

    /// <summary>
    /// Mints a bearer token with author claims used by <c>EnsureAuthorMiddleware</c> and command handlers.
    /// </summary>
    internal static string CreateAuthorToken(
        string subject = "acceptance-test-author",
        string? displayName = "Acceptance Test Author")
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(DevJwtSecret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, subject),
            new(ClaimTypes.NameIdentifier, subject),
            new("name", displayName ?? subject),
        };

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
