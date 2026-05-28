namespace LitePress.Integration.Tests.Infrastructure;

/// <summary>
/// xUnit collection that shares a single <see cref="ApiIntegrationFixture"/> (one PostgreSQL container and API host)
/// across integration test classes. Apply with <c>[Collection(ApiIntegrationCollection.Name)]</c>.
/// </summary>
[CollectionDefinition(Name)]
public sealed class ApiIntegrationCollection : ICollectionFixture<ApiIntegrationFixture>
{
    /// <summary>Collection name referenced by <see cref="Xunit.CollectionAttribute"/>.</summary>
    public const string Name = "ApiIntegration";
}
