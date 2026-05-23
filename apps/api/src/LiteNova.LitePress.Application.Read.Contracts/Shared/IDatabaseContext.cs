namespace LiteNova.LitePress.Application.Read.Contracts.Shared;

public interface IDatabaseContext
{
    public IQueryable<Post> Posts { get; }
    public IQueryable<Tag> Tags { get; }
    public IQueryable<Author> Authors { get; }
}
