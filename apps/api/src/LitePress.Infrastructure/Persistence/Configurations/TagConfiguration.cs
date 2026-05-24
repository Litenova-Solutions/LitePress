using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LitePress.Infrastructure.Persistence.Configurations;

internal sealed class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.ToTable("tags");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .HasConversion(id => id.Value, value => new TagId(value))
            .HasColumnName("id");

        builder.OwnsOne(t => t.Name, b =>
        {
            b.Property(n => n.Value).HasColumnName("name").HasMaxLength(50).IsRequired();
            b.HasIndex(n => n.Value).IsUnique().HasDatabaseName("uq_tags_name");
        });

        builder.OwnsOne(t => t.Slug, b =>
        {
            b.Property(s => s.Value).HasColumnName("slug").HasMaxLength(100).IsRequired();
            b.HasIndex(s => s.Value).IsUnique().HasDatabaseName("uq_tags_slug");
        });

        builder.Property(t => t.CreatedAt).HasColumnName("created_at").IsRequired();
    }
}