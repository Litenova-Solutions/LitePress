using LiteBus.Commands.Abstractions;
using LitePress.Infrastructure.Persistence;

namespace LitePress.Infrastructure.Behaviors;

internal sealed class SaveChangesCommandPostHandler : ICommandPostHandler<ICommand>
{
    private readonly LitePressDbContext _context;

    public SaveChangesCommandPostHandler(LitePressDbContext context)
    {
        _context = context;
    }

    public async Task PostHandleAsync(ICommand command, object? result, CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
        await _context.Database.CommitTransactionAsync(cancellationToken);
    }
}
