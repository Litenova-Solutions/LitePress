using LiteNova.Blog.Domain.Posts;
using LiteNova.Blog.Domain.Tags;
using Microsoft.EntityFrameworkCore;

namespace LiteNova.Blog.Application.Common.Interfaces;

public interface IBlogDbContext
{
    DbSet<Post> Posts { get; }
    DbSet<Tag> Tags { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
