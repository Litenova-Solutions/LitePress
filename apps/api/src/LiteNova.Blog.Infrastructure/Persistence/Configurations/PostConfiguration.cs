using LiteNova.Blog.Domain.Posts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LiteNova.Blog.Infrastructure.Persistence.Configurations;

public sealed class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.ToTable("posts");
        builder.HasKey(p => p.Id);
        builder.HasIndex(p => p.Slug).IsUnique();
        builder.Property(p => p.Status).HasConversion<string>();
        builder.OwnsMany(p => p.Tags, tags =>
        {
            tags.ToTable("post_tags");
            tags.WithOwner().HasForeignKey(t => t.PostId);
            tags.HasKey(t => new { t.PostId, t.TagId });
        });
    }
}
