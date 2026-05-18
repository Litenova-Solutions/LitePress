using LiteBus.Commands.Abstractions;
using LiteBus.Queries.Abstractions;
using LiteNova.Blog.Application.Common.Exceptions;

namespace LiteNova.Blog.Application.Common.Behaviors;

public sealed class ValidationBehavior
{
    public static async Task ValidateCommandAsync<TCommand>(
        TCommand command,
        IEnumerable<ICommandValidator<TCommand>> validators,
        CancellationToken cancellationToken)
        where TCommand : ICommand
    {
        var failures = new Dictionary<string, string[]>();
        foreach (var validator in validators)
        {
            try
            {
                await validator.ValidateAsync(command, cancellationToken);
            }
            catch (Exception ex)
            {
                failures[validator.GetType().Name] = [ex.Message];
            }
        }

        if (failures.Count > 0)
        {
            throw new ValidationException(failures);
        }
    }

    public static async Task ValidateQueryAsync<TQuery>(
        TQuery query,
        IEnumerable<IQueryValidator<TQuery>> validators,
        CancellationToken cancellationToken)
        where TQuery : IQuery
    {
        var failures = new Dictionary<string, string[]>();
        foreach (var validator in validators)
        {
            try
            {
                await validator.ValidateAsync(query, cancellationToken);
            }
            catch (Exception ex)
            {
                failures[validator.GetType().Name] = [ex.Message];
            }
        }

        if (failures.Count > 0)
        {
            throw new ValidationException(failures);
        }
    }
}
