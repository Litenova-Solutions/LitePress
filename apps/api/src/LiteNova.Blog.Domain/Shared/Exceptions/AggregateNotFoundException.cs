namespace LiteNova.Blog.Domain.Shared.Exceptions;

public abstract class AggregateNotFoundException : DomainException
{
    protected AggregateNotFoundException(string message)
        : base(message) { }
}
