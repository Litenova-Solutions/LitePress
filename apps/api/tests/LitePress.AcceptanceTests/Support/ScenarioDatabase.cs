using LitePress.Domain.Authors;
using LitePress.Domain.Posts;
using LitePress.Domain.Tags;
using Marten;

namespace LitePress.AcceptanceTests.Support;

/// <summary>
/// Marten helpers for acceptance test isolation. Deletes all aggregate documents between BDD scenarios
/// so steps do not leak data from prior scenarios in the same test run.
/// </summary>
internal static class ScenarioDatabase
{
    /// <summary>Removes every <see cref="Post"/>, <see cref="Tag"/>, and <see cref="Author"/> document in the test database.</summary>
    internal static async Task ResetAsync(string connectionString, CancellationToken cancellationToken = default)
    {
        await using var store = DocumentStore.For(options =>
        {
            options.Connection(connectionString);
            LitePress.Infrastructure.Marten.MartenStoreRegistration.ConfigureStore(options);
        });

        await using var session = store.LightweightSession();
        session.DeleteWhere<Post>(_ => true);
        session.DeleteWhere<Tag>(_ => true);
        session.DeleteWhere<Author>(_ => true);
        await session.SaveChangesAsync(cancellationToken);
    }
}
