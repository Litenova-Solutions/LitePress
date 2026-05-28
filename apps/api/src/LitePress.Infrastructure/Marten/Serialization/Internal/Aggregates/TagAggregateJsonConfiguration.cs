using LitePress.Domain.Tags;
using LitePress.Infrastructure.Marten.Serialization.Abstractions.Configurations;

namespace LitePress.Infrastructure.Marten.Serialization.Internal.Aggregates;

/// <summary>
/// JSON rules for the <see cref="Tag"/> aggregate document.
/// </summary>
internal sealed class TagAggregateJsonConfiguration : AggregateRootJsonConfiguration<Tag>;
