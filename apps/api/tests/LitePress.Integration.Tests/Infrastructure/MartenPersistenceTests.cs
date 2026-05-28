using LitePress.Domain.Authors;
using LitePress.Domain.Posts;
using Microsoft.Extensions.DependencyInjection;

namespace LitePress.Integration.Tests.Infrastructure;

/// <summary>
/// Verifies Marten can persist and load a <see cref="Post"/> aggregate through <see cref="IPostRepository"/>
/// using the same store configuration as production.
/// </summary>
[Collection(ApiIntegrationCollection.Name)]
public sealed class MartenPersistenceTests
{
    private readonly ApiIntegrationFixture _fixture;

    public MartenPersistenceTests(ApiIntegrationFixture fixture)
    {
        _fixture = fixture;
    }

    /// <summary>Store via repository and load by id after <see cref="Marten.IDocumentSession.SaveChangesAsync"/>.</summary>
    [Fact]
    public async Task StorePost_CanBeLoadedByRepository()
    {
        await using var scope = _fixture.Factory.Services.CreateAsyncScope();
        var repository = scope.ServiceProvider.GetRequiredService<IPostRepository>();
        var session = scope.ServiceProvider.GetRequiredService<Marten.IDocumentSession>();

        var postId = PostId.New();
        var authorId = AuthorId.New();
        var post = Post.Create(
            postId,
            new PostTitle("Diagnostic title"),
            new PostContent("{}"),
            authorId,
            DateTimeOffset.UtcNow);

        await repository.AddAsync(post, CancellationToken.None);
        await session.SaveChangesAsync(CancellationToken.None);

        var loaded = await repository.GetByIdAsync(postId, CancellationToken.None);
        loaded.Title.Value.Should().Be("Diagnostic title");
    }
}
