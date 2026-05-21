using LiteBus.Commands.Abstractions;
using LiteNova.Blog.Infrastructure.Persistence;

namespace LiteNova.Blog.Infrastructure.Behaviors;

internal sealed class RollbackCommandErrorHandler : ICommandErrorHandler<ICommand>
{
    private readonly BlogDbContext _context;

    public RollbackCommandErrorHandler(BlogDbContext context)
    {
        _context = context;
    }

    public async Task HandleErrorAsync(ICommand command, object? commandResult, Exception exception, CancellationToken cancellationToken)
    {
        if (_context.Database.CurrentTransaction is not null)
        {
            await _context.Database.RollbackTransactionAsync(cancellationToken);
        }

        throw exception;
    }
}
