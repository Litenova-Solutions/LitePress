namespace LiteNova.Blog.Application.Common.Exceptions;

public sealed class ValidationException(IReadOnlyDictionary<string, string[]> errors) : ApplicationException("Validation failed.")
{
    public IReadOnlyDictionary<string, string[]> Errors { get; } = errors;
}
