using LiteBus.Commands.Abstractions;
using LiteNova.Blog.Infrastructure.Persistence;

namespace LiteNova.Blog.Infrastructure.Behaviors;

internal sealed class SaveChangesCommandPostHandler : ICommandPostHandler<ICommand>
{
    private readonly BlogDbContext _context;

    public SaveChangesCommandPostHandler(BlogDbContext context)
    {
        _context = context;
    }

    public async Task PostHandleAsync(ICommand command, object? result, CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
        await _context.Database.CommitTransactionAsync(cancellationToken);
    }
}
