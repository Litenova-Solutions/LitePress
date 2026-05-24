namespace LitePress.Application.Read.Contracts.Shared.Exceptions;

public abstract class QueryValidationException : Exception
{
    protected QueryValidationException(string message)
        : base(message) { }
}
