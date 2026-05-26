namespace LitePress.Application.Write.Contracts.Shared;

/// <summary>Provides the current UTC time for write handlers and aggregate mutations.</summary>
public interface IClock
{
    /// <summary>Current UTC time.</summary>
    public DateTimeOffset UtcNow { get; }
}
