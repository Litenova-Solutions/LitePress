using System.Text.RegularExpressions;
using LiteNova.Blog.Domain.Common;
using LiteNova.Blog.Domain.Posts.Events;
using LiteNova.Blog.Domain.Posts.Exceptions;
using LiteNova.Blog.Domain.Tags;

namespace LiteNova.Blog.Domain.Posts;

/// <summary>
/// Post aggregate root responsible for post lifecycle and invariants.
/// </summary>
public class Post : AggregateRoot
{
    private readonly List<PostTag> _tags = [];

    public string Title { get; private set; } = string.Empty;
    public string Slug { get; private set; } = string.Empty;
    public string Excerpt { get; private set; } = string.Empty;
    public string Body { get; private set; } = string.Empty;
    public string? CoverImageUrl { get; private set; }
    public PostStatus Status { get; private set; } = PostStatus.Draft;
    public DateTimeOffset? PublishedAt { get; private set; }
    public DateTimeOffset? ScheduledFor { get; private set; }
    public int ReadingTimeMinutes { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; private set; } = DateTimeOffset.UtcNow;
    public IReadOnlyCollection<PostTag> Tags => _tags.AsReadOnly();

    private Post()
    {
    }

    public static Post Create(string title, string excerpt, string body, string? coverImageUrl, IEnumerable<Tag> tags)
    {
        var post = new Post
        {
            Title = title.Trim(),
            Excerpt = excerpt.Trim(),
            Body = body,
            CoverImageUrl = coverImageUrl,
            Slug = CreateSlug(title),
            ReadingTimeMinutes = CalculateReadingTime(body)
        };

        post.SetTags(tags);
        post.RaiseDomainEvent(new PostCreatedEvent(post.Id));
        return post;
    }

    public void Update(string title, string excerpt, string body, string? coverImageUrl, IEnumerable<Tag> tags)
    {
        Title = title.Trim();
        Excerpt = excerpt.Trim();
        Body = body;
        CoverImageUrl = coverImageUrl;
        Slug = CreateSlug(title);
        ReadingTimeMinutes = CalculateReadingTime(body);
        UpdatedAt = DateTimeOffset.UtcNow;
        SetTags(tags);
        RaiseDomainEvent(new PostUpdatedEvent(Id));
    }

    public void Publish()
    {
        if (Status is PostStatus.Published)
        {
            throw new PostAlreadyPublishedException(Id);
        }

        Status = PostStatus.Published;
        PublishedAt = DateTimeOffset.UtcNow;
        ScheduledFor = null;
        UpdatedAt = DateTimeOffset.UtcNow;
        RaiseDomainEvent(new PostPublishedEvent(Id));
    }

    public void Schedule(DateTimeOffset scheduledFor)
    {
        if (Status is PostStatus.Scheduled)
        {
            throw new PostAlreadyScheduledException(Id);
        }

        Status = PostStatus.Scheduled;
        ScheduledFor = scheduledFor;
        UpdatedAt = DateTimeOffset.UtcNow;
        RaiseDomainEvent(new PostScheduledEvent(Id, scheduledFor));
    }

    public void Delete() => RaiseDomainEvent(new PostDeletedEvent(Id));

    private void SetTags(IEnumerable<Tag> tags)
    {
        _tags.Clear();
        _tags.AddRange(tags.Select(tag => new PostTag { PostId = Id, TagId = tag.Id }));
    }

    private static string CreateSlug(string title)
    {
        var normalized = Regex.Replace(title.ToLowerInvariant().Trim(), @"[^a-z0-9\s-]", string.Empty);
        var slug = Regex.Replace(normalized, @"\s+", "-");
        if (string.IsNullOrWhiteSpace(slug))
        {
            throw new InvalidPostSlugException(title);
        }

        return slug;
    }

    private static int CalculateReadingTime(string body)
    {
        var words = Math.Max(1, Regex.Matches(body, @"\w+").Count);
        return Math.Max(1, (int)Math.Ceiling(words / 200d));
    }
}
