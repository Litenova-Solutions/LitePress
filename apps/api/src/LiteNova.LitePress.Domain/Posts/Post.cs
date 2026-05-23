using LiteNova.LitePress.Domain.Authors;
using LiteNova.LitePress.Domain.Posts.Events;
using LiteNova.LitePress.Domain.Posts.Exceptions;
using LiteNova.LitePress.Domain.Shared;
using LiteNova.LitePress.Domain.Tags;

namespace LiteNova.LitePress.Domain.Posts;

public sealed class Post : AggregateRoot<PostId>
{
    private readonly List<PostTag> _tags = [];
    private string _stateType = "Draft";
    private DateTimeOffset? _publishedAt;
    private DateTimeOffset? _archivedAt;

    private Post() { }

    public PostTitle Title { get; private set; } = null!;
    public PostSlug Slug { get; private set; } = null!;
    public PostContent Content { get; private set; } = null!;
    public PostExcerpt? Excerpt { get; private set; }
    public PostCoverImageUrl? CoverImageUrl { get; private set; }
    public AuthorId AuthorId { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }
    public IReadOnlyList<PostTag> Tags => _tags.AsReadOnly();

    public PostState State => _stateType switch
    {
        "Draft" => new DraftPostState(),
        "Published" => new PublishedPostState(_publishedAt!.Value),
        "Archived" => new ArchivedPostState(_archivedAt!.Value),
        _ => throw new InvalidOperationException($"Unknown state type: {_stateType}")
    };

    public DateTimeOffset? PublishedAt => _publishedAt;
    public DateTimeOffset? ArchivedAt => _archivedAt;

    public static Post Create(
        PostId id,
        PostTitle title,
        PostContent content,
        AuthorId authorId,
        PostExcerpt? excerpt = null,
        PostCoverImageUrl? coverImageUrl = null,
        IReadOnlyList<TagId>? tagIds = null)
    {
        var now = DateTimeOffset.UtcNow;
        var slug = PostSlug.FromTitle(title.Value);

        var post = new Post
        {
            Id = id,
            Title = title,
            Content = content,
            AuthorId = authorId,
            Excerpt = excerpt,
            CoverImageUrl = coverImageUrl,
            Slug = slug,
            _stateType = "Draft",
            CreatedAt = now,
            UpdatedAt = now
        };

        foreach (var tagId in tagIds ?? [])
        {
            post._tags.Add(new PostTag(id, tagId));
        }

        post.RaiseDomainEvent(new PostCreated(id, authorId, title, slug, content, excerpt, coverImageUrl, tagIds ?? []));
        return post;
    }

    public void Update(
        PostTitle title,
        PostContent content,
        PostExcerpt? excerpt,
        PostCoverImageUrl? coverImageUrl)
    {
        if (_stateType != "Draft")
        {
            throw new PostNotEditableException(Id);
        }

        Title = title;
        Content = content;
        Excerpt = excerpt;
        CoverImageUrl = coverImageUrl;
        Slug = PostSlug.FromTitle(title.Value);
        UpdatedAt = DateTimeOffset.UtcNow;

        RaiseDomainEvent(new PostUpdated(Id, title, Slug, content, excerpt, coverImageUrl));
    }

    public void Publish()
    {
        if (_stateType == "Published")
        {
            throw new PostAlreadyPublishedException(Id);
        }

        if (_stateType != "Draft")
        {
            throw new PostNotEditableException(Id);
        }

        var now = DateTimeOffset.UtcNow;
        _stateType = "Published";
        _publishedAt = now;
        UpdatedAt = now;

        RaiseDomainEvent(new PostPublished(Id, AuthorId, now));
    }

    public void Archive()
    {
        if (_stateType == "Archived")
        {
            throw new PostAlreadyArchivedException(Id);
        }

        var now = DateTimeOffset.UtcNow;
        _stateType = "Archived";
        _archivedAt = now;
        UpdatedAt = now;

        RaiseDomainEvent(new PostArchived(Id, now));
    }

    public void Delete()
    {
        if (_stateType == "Published")
        {
            throw new PostCannotBeDeletedException(Id);
        }

        RaiseDomainEvent(new PostDeleted(Id));
    }

    public void AddTag(TagId tagId)
    {
        if (_tags.Count >= 10)
        {
            throw new PostTagLimitExceededException(Id);
        }

        if (_tags.Any(t => t.TagId == tagId))
        {
            throw new PostTagAlreadyAssignedException(Id, tagId);
        }

        _tags.Add(new PostTag(Id, tagId));
        RaiseDomainEvent(new PostTagAdded(Id, tagId));
    }

    public void RemoveTag(TagId tagId)
    {
        var tag = _tags.FirstOrDefault(t => t.TagId == tagId)
            ?? throw new PostTagNotAssignedException(Id, tagId);

        _tags.Remove(tag);
        RaiseDomainEvent(new PostTagRemoved(Id, tagId));
    }
}
