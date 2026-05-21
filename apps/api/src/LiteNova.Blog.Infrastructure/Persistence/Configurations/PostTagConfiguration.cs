using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LiteNova.Blog.Infrastructure.Persistence.Configurations;

internal sealed class PostTagConfiguration : IEntityTypeConfiguration<PostTag>
{
    public void Configure(EntityTypeBuilder<PostTag> builder)
    {
        builder.ToTable("post_tags");

        builder.Property(pt => pt.PostId)
            .HasConversion(id => id.Value, value => new PostId(value))
            .HasColumnName("post_id")
            .IsRequired();

        builder.Property(pt => pt.TagId)
            .HasConversion(id => id.Value, value => new TagId(value))
            .HasColumnName("tag_id")
            .IsRequired();

        builder.HasKey(pt => new { pt.PostId, pt.TagId });

        builder.HasIndex(pt => pt.TagId).HasDatabaseName("ix_post_tags_tag_id");
    }
}