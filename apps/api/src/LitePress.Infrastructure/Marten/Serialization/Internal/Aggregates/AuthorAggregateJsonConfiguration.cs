using LitePress.Domain.Authors;
using LitePress.Infrastructure.Marten.Serialization.Abstractions.Configurations;

namespace LitePress.Infrastructure.Marten.Serialization.Internal.Aggregates;

/// <summary>
/// JSON rules for the <see cref="Author"/> aggregate document.
/// </summary>
internal sealed class AuthorAggregateJsonConfiguration : AggregateRootJsonConfiguration<Author>;
