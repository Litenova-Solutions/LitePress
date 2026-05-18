using LiteBus.Commands.Abstractions;
using LiteBus.Events.Abstractions;
using LiteNova.Blog.Application.Common.Interfaces;
using LiteNova.Blog.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace LiteNova.Blog.Application.Common.PostHandlers;

/// <summary>Dispatches domain events raised by aggregates after any void command is handled.</summary>
internal sealed class DomainEventDispatchCommandPostHandler<TCommand>(IBlogDbContext dbContext, IEventMediator eventMediator)
    : ICommandPostHandler<TCommand>
    where TCommand : ICommand
{
    public async Task PostHandleAsync(TCommand message, object? messageResult, CancellationToken cancellationToken)
    {
        if (dbContext is not DbContext efDbContext) { return; }
        var aggregates = efDbContext.ChangeTracker.Entries<AggregateRoot>().Select(e => e.Entity).ToArray();
        var events = aggregates.SelectMany(a => a.GetDomainEvents()).ToArray();
        foreach (var aggregate in aggregates) { aggregate.ClearDomainEvents(); }
        foreach (var domainEvent in events) { await eventMediator.PublishAsync((dynamic)domainEvent, cancellationToken: cancellationToken); }
    }
}

/// <summary>Dispatches domain events raised by aggregates after any command with a result is handled.</summary>
internal sealed class DomainEventDispatchCommandResultPostHandler<TCommand, TResult>(IBlogDbContext dbContext, IEventMediator eventMediator)
    : ICommandPostHandler<TCommand, TResult>
    where TCommand : ICommand<TResult>
    where TResult : notnull
{
    public async Task PostHandleAsync(TCommand message, TResult? result, CancellationToken cancellationToken)
    {
        if (dbContext is not DbContext efDbContext) { return; }
        var aggregates = efDbContext.ChangeTracker.Entries<AggregateRoot>().Select(e => e.Entity).ToArray();
        var events = aggregates.SelectMany(a => a.GetDomainEvents()).ToArray();
        foreach (var aggregate in aggregates) { aggregate.ClearDomainEvents(); }
        foreach (var domainEvent in events) { await eventMediator.PublishAsync((dynamic)domainEvent, cancellationToken: cancellationToken); }
    }
}
