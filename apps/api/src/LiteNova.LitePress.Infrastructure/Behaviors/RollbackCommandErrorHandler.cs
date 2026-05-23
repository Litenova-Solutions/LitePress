using LiteBus.Commands.Abstractions;
using LiteNova.LitePress.Infrastructure.Persistence;

namespace LiteNova.LitePress.Infrastructure.Behaviors;

internal sealed class RollbackCommandErrorHandler : ICommandErrorHandler<ICommand>
{
    private readonly LitePressDbContext _context;

    public RollbackCommandErrorHandler(LitePressDbContext context)
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
