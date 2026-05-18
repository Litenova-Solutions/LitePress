using LiteNova.Blog.Application.Common.Interfaces;

namespace LiteNova.Blog.Application.Common.Behaviors;

public sealed class UnitOfWorkBehavior(IBlogDbContext dbContext)
{
    public Task CommitAsync(CancellationToken cancellationToken) => dbContext.SaveChangesAsync(cancellationToken);
}
