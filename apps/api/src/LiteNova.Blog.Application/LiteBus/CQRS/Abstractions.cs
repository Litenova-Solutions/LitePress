namespace LiteBus.CQRS;

public interface ICommand;
public interface ICommand<out TResult> : ICommand;
public interface IQuery<out TResult>;

public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
    Task HandleAsync(TCommand command, CancellationToken cancellationToken);
}

public interface ICommandHandler<in TCommand, TResult> where TCommand : ICommand<TResult>
{
    Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken);
}

public interface IQueryHandler<in TQuery, TResult> where TQuery : IQuery<TResult>
{
    Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken);
}

public interface ICommandValidator<in TCommand>
{
    Task ValidateAsync(TCommand command, CancellationToken cancellationToken);
}

public interface IQueryValidator<in TQuery>
{
    Task ValidateAsync(TQuery query, CancellationToken cancellationToken);
}

public interface IMessageBus
{
    Task<TResult> SendAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default);
    Task SendAsync(ICommand command, CancellationToken cancellationToken = default);
    Task<TResult> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default);
    Task PublishAsync(object domainEvent, CancellationToken cancellationToken = default);
}

public interface IDomainEventHandler<in TEvent>
{
    Task HandleAsync(TEvent domainEvent, CancellationToken cancellationToken);
}
