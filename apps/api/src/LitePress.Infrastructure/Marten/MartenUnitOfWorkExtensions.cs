using LitePress.Domain.Shared;

namespace LitePress.Infrastructure.Marten;

/// <summary>
/// Stages Marten session changes and registers aggregates for domain event dispatch after save.
/// </summary>
internal static class MartenUnitOfWorkExtensions
{
    internal static void StoreAndTrack<T>(this IMartenUnitOfWork unitOfWork, T aggregate)
        where T : class, IAggregateRoot
    {
        unitOfWork.Session.Store(aggregate);
        unitOfWork.Track(aggregate);
    }

    internal static void DeleteAndTrack<T>(this IMartenUnitOfWork unitOfWork, T aggregate)
        where T : class, IAggregateRoot
    {
        unitOfWork.Track(aggregate);
        unitOfWork.Session.Delete(aggregate);
    }
}
