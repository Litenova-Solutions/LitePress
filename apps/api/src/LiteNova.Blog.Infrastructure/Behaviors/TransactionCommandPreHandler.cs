using LiteBus.Commands.Abstractions;
using LiteBus.Messaging.Abstractions;
using LiteNova.Blog.Infrastructure.Persistence;

namespace LiteNova.Blog.Infrastructure.Behaviors;

[HandlerPriority(10)]
internal sealed class TransactionCommandPreHandler : ICommandPreHandler<ICommand>
{
    private readonly BlogDbContext _context;

    public TransactionCommandPreHandler(BlogDbContext context)
    {
        _context = context;
    }

    public async Task PreHandleAsync(ICommand command, CancellationToken cancellationToken)
    {
        await _context.Database.BeginTransactionAsync(cancellationToken);
    }
}
