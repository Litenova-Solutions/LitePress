using LitePress.Application.Read.Contracts.Posts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LitePress.Infrastructure.Persistence.Configurations;

internal sealed class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.ToTable("posts");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion(id => id.Value, value => new PostId(value))
            .HasColumnName("id");

        builder.Property(p => p.AuthorId)
            .HasConversion(id => id.Value, value => new AuthorId(value))
            .HasColumnName("author_id")
            .IsRequired();

        builder.OwnsOne(p => p.Title, b =>
        {
            b.Property(t => t.Value).HasColumnName("title").HasMaxLength(200).IsRequired();
        });

        builder.OwnsOne(p => p.Slug, b =>
        {
            b.Property(s => s.Value).HasColumnName("slug").HasMaxLength(300).IsRequired();
            b.HasIndex(s => s.Value).IsUnique().HasDatabaseName("uq_posts_slug");
        });

        builder.OwnsOne(p => p.Content, b =>
        {
            b.Property(c => c.Value).HasColumnName("content").IsRequired();
        });

        builder.OwnsOne(p => p.Excerpt, b =>
        {
            b.Property(e => e.Value).HasColumnName("excerpt").HasMaxLength(500);
        });

        builder.OwnsOne(p => p.CoverImageUrl, b =>
        {
            b.Property(u => u.Value).HasColumnName("cover_image_url").HasMaxLength(2048);
        });

        builder.Property(p => p.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.Property(p => p.UpdatedAt).HasColumnName("updated_at").IsRequired();

        builder.Ignore(p => p.State);

        builder.Property<string>(PostStateColumns.StateType)
            .HasColumnName("state_type")
            .HasMaxLength(20)
            .IsRequired()
            .HasDefaultValue(PostStateColumns.Draft);

        builder.Property<DateTimeOffset?>(PostStateColumns.PublishedAt)
            .HasColumnName("published_at");

        builder.Property<DateTimeOffset?>(PostStateColumns.ArchivedAt)
            .HasColumnName("archived_at");

        builder.HasIndex(PostStateColumns.StateType).HasDatabaseName("ix_posts_state_type");
        builder.HasIndex(PostStateColumns.PublishedAt).HasDatabaseName("ix_posts_published_at");

        builder.HasMany(p => p.Tags)
            .WithOne()
            .HasForeignKey(pt => pt.PostId)
            .IsRequired();

        builder.Navigation(p => p.Tags).HasField("_tags").UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasIndex(p => p.AuthorId).HasDatabaseName("ix_posts_author_id");
    }
}
