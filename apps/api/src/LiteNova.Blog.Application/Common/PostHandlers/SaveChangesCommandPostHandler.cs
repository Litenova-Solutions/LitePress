using LiteBus.Commands.Abstractions;
using LiteNova.Blog.Application.Common.Interfaces;

namespace LiteNova.Blog.Application.Common.PostHandlers;

/// <summary>Saves pending changes to the database after any command is handled.</summary>
internal sealed class SaveChangesCommandPostHandler<TCommand>(IBlogDbContext dbContext) : ICommandPostHandler<TCommand>
    where TCommand : ICommand
{
    public Task PostHandleAsync(TCommand message, object? messageResult, CancellationToken cancellationToken) =>
        dbContext.SaveChangesAsync(cancellationToken);
}

/// <summary>Saves pending changes to the database after any command with a result is handled.</summary>
internal sealed class SaveChangesCommandResultPostHandler<TCommand, TResult>(IBlogDbContext dbContext) : ICommandPostHandler<TCommand, TResult>
    where TCommand : ICommand<TResult>
    where TResult : notnull
{
    public Task PostHandleAsync(TCommand message, TResult? result, CancellationToken cancellationToken) =>
        dbContext.SaveChangesAsync(cancellationToken);
}
