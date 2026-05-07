using LiteBus.CQRS;
using LiteNova.Blog.Application.Common.Exceptions;

namespace LiteNova.Blog.Application.Common.Behaviors;

public sealed class ValidationBehavior
{
    public static async Task ValidateCommandAsync<TCommand>(TCommand command, IEnumerable<ICommandValidator<TCommand>> validators, CancellationToken cancellationToken)
    {
        var failures = new Dictionary<string, string[]>();
        foreach (var validator in validators)
        {
            await validator.ValidateAsync(command, cancellationToken);
        }
        if (failures.Count > 0) throw new ValidationException(failures);
    }

    public static async Task ValidateQueryAsync<TQuery>(TQuery query, IEnumerable<IQueryValidator<TQuery>> validators, CancellationToken cancellationToken)
    {
        foreach (var validator in validators)
        {
            await validator.ValidateAsync(query, cancellationToken);
        }
    }
}
