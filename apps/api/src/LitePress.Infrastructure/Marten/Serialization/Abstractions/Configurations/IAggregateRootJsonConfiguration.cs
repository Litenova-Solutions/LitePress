namespace LitePress.Infrastructure.Marten.Serialization.Abstractions.Configurations;

/// <summary>
/// JSON serialization for one aggregate root document and related types embedded in that document.
/// </summary>
internal interface IAggregateRootJsonConfiguration : IJsonTypeConfiguration
{
    public Type AggregateRootType { get; }
}
