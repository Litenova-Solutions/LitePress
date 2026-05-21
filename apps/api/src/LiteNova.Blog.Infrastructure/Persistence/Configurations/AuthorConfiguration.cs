using LiteNova.Blog.Domain.Authors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LiteNova.Blog.Infrastructure.Persistence.Configurations;

internal sealed class AuthorConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.ToTable("authors");
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .HasConversion(id => id.Value, value => new AuthorId(value))
            .HasColumnName("id");

        builder.Property(a => a.ExternalId)
            .HasColumnName("external_id")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(a => a.DisplayName)
            .HasColumnName("display_name")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(a => a.RegisteredAt)
            .HasColumnName("registered_at")
            .IsRequired();

        builder.HasIndex(a => a.ExternalId)
            .IsUnique()
            .HasDatabaseName("uq_authors_external_id");
    }
}
