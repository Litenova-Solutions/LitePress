using LiteBus.Commands.Abstractions;
using LiteBus.Messaging.Abstractions;
using LiteNova.LitePress.Infrastructure.Persistence;

namespace LiteNova.LitePress.Infrastructure.Behaviors;

[HandlerPriority(10)]
internal sealed class TransactionCommandPreHandler : ICommandPreHandler<ICommand>
{
    private readonly LitePressDbContext _context;

    public TransactionCommandPreHandler(LitePressDbContext context)
    {
        _context = context;
    }

    public async Task PreHandleAsync(ICommand command, CancellationToken cancellationToken)
    {
        await _context.Database.BeginTransactionAsync(cancellationToken);
    }
}
