using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace LitePress.Integration.Tests.Infrastructure;

internal static class TestAuth
{
    internal const string DevJwtSecret = "dev-secret-key-must-be-at-least-32-characters-long!";

    internal static string CreateBearerToken(string subject = "integration-test-user", string? name = "Integration Test User")
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(DevJwtSecret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, subject),
            new(ClaimTypes.NameIdentifier, subject),
            new("name", name ?? subject),
        };

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    internal static HttpRequestMessage WithBearer(this HttpRequestMessage request, string? token = null)
    {
        request.Headers.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token ?? CreateBearerToken());
        return request;
    }
}
