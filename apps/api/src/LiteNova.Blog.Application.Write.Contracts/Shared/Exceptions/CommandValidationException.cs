namespace LiteNova.Blog.Application.Write.Contracts.Shared.Exceptions;

public abstract class CommandValidationException : Exception
{
    protected CommandValidationException(string message)
        : base(message) { }
}
