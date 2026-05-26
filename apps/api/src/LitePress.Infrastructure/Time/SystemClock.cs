using LitePress.Application.Write.Contracts.Shared;

namespace LitePress.Infrastructure.Time;

internal sealed class SystemClock : IClock
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
