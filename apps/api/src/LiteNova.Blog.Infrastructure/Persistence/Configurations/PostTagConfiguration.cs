using LiteNova.Blog.Domain.Posts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LiteNova.Blog.Infrastructure.Persistence.Configurations;

public sealed class PostTagConfiguration : IEntityTypeConfiguration<PostTag>
{
    public void Configure(EntityTypeBuilder<PostTag> builder)
    {
        builder.ToTable("post_tags");
        builder.HasKey(pt => new { pt.PostId, pt.TagId });
    }
}
