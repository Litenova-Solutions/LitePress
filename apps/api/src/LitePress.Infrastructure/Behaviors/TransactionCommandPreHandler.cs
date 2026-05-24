using LiteBus.Commands.Abstractions;
using LiteBus.Messaging.Abstractions;
using LitePress.Infrastructure.Persistence;

namespace LitePress.Infrastructure.Behaviors;

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
