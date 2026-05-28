using Microsoft.Extensions.Configuration;

namespace LitePress.Infrastructure.Marten;

/// <summary>
/// Connection string resolution for Marten and PostgreSQL.
/// </summary>
internal static class MartenConnection
{
    internal const string DefaultConnectionName = "DefaultConnection";

    internal static string GetRequired(IConfiguration configuration) =>
        configuration.GetConnectionString(DefaultConnectionName)
        ?? throw new InvalidOperationException($"ConnectionStrings:{DefaultConnectionName} is required.");
}
