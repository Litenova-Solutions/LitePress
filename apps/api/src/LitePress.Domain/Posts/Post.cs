using LitePress.Domain.Authors;
using LitePress.Domain.Authors.Exceptions;
using LitePress.Domain.Posts.Events;
using LitePress.Domain.Posts.Exceptions;
using LitePress.Domain.Shared;
using LitePress.Domain.Tags;

namespace LitePress.Domain.Posts;

/// <summary>Post aggregate root. Owns content, lifecycle state, and tag assignments.</summary>
public sealed class Post : AggregateRoot<PostId>
{
    private readonly List<PostTag> _tags = [];

    private Post() { }

    /// <summary>The post title.</summary>
    public PostTitle Title { get; private set; } = null!;

    /// <summary>The URL slug derived from the title.</summary>
    public PostSlug Slug { get; private set; } = null!;

    /// <summary>The post body stored as ProseMirror JSON.</summary>
    public PostContent Content { get; private set; } = null!;

    /// <summary>Optional short summary for listings.</summary>
    public PostExcerpt? Excerpt { get; private set; }

    /// <summary>Optional cover image URL.</summary>
    public PostCoverImageUrl? CoverImageUrl { get; private set; }

    /// <summary>The author who owns this post.</summary>
    public AuthorId AuthorId { get; private set; }

    /// <summary>When the post was created.</summary>
    public DateTimeOffset CreatedAt { get; private set; }

    /// <summary>When the post was last mutated.</summary>
    public DateTimeOffset UpdatedAt { get; private set; }

    /// <summary>Current lifecycle state.</summary>
    public PostState State { get; private set; } = new DraftPostState();

    /// <summary>Tags assigned to this post.</summary>
    public IReadOnlyList<PostTag> Tags => _tags.AsReadOnly();

    /// <summary>Creates a new draft post.</summary>
    /// <param name="utcNow">Current UTC time passed from the application handler.</param>
    public static Post Create(
        PostId id,
        PostTitle title,
        PostContent content,
        AuthorId authorId,
        DateTimeOffset utcNow,
        PostExcerpt? excerpt = null,
        PostCoverImageUrl? coverImageUrl = null,
        IReadOnlyList<TagId>? tagIds = null)
    {
        if (id == default)
        {
            throw new PostIdentityRequiredException();
        }

        if (authorId == default)
        {
            throw new AuthorIdentityRequiredException();
        }

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
            State = new DraftPostState(),
            CreatedAt = utcNow,
            UpdatedAt = utcNow
        };

        foreach (var tagId in tagIds ?? [])
        {
            post._tags.Add(new PostTag(id, tagId));
        }

        post.RaiseDomainEvent(new PostCreated(
            id,
            authorId,
            title,
            slug,
            content,
            excerpt,
            coverImageUrl,
            tagIds ?? []));

        return post;
    }

    /// <summary>Updates draft content. Only permitted while the post is a draft.</summary>
    /// <param name="utcNow">Current UTC time from the handler.</param>
    public void Update(
        PostTitle title,
        PostContent content,
        PostExcerpt? excerpt,
        PostCoverImageUrl? coverImageUrl,
        DateTimeOffset utcNow)
    {
        switch (State)
        {
            case PublishedPostState:
                throw new PostAlreadyPublishedException(Id);
            case ArchivedPostState:
                throw new PostNotEditableException(Id);
            case DraftPostState:
                break;
        }

        Title = title;
        Content = content;
        Excerpt = excerpt;
        CoverImageUrl = coverImageUrl;
        Slug = PostSlug.FromTitle(title.Value);
        UpdatedAt = utcNow;

        RaiseDomainEvent(new PostUpdated(Id, title, Slug, content, excerpt, coverImageUrl));
    }

    /// <summary>Publishes the post, making it visible to readers.</summary>
    /// <param name="utcNow">Current UTC time from the handler.</param>
    public void Publish(DateTimeOffset utcNow)
    {
        switch (State)
        {
            case PublishedPostState:
                throw new PostAlreadyPublishedException(Id);
            case ArchivedPostState:
                throw new PostNotEditableException(Id);
            case DraftPostState:
                State = new PublishedPostState(utcNow);
                UpdatedAt = utcNow;
                RaiseDomainEvent(new PostPublished(Id, AuthorId, utcNow));
                break;
        }
    }

    /// <summary>Archives the post, removing it from active publication.</summary>
    /// <param name="utcNow">Current UTC time from the handler.</param>
    public void Archive(DateTimeOffset utcNow)
    {
        if (State is ArchivedPostState)
        {
            throw new PostAlreadyArchivedException(Id);
        }

        State = new ArchivedPostState(utcNow);
        UpdatedAt = utcNow;

        RaiseDomainEvent(new PostArchived(Id, utcNow));
    }

    /// <summary>Marks the post for deletion. Published posts cannot be deleted.</summary>
    public void Delete()
    {
        if (State is PublishedPostState)
        {
            throw new PostCannotBeDeletedException(Id);
        }

        RaiseDomainEvent(new PostDeleted(Id));
    }

    /// <summary>Assigns a tag to this post.</summary>
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

    /// <summary>Removes a tag assignment from this post.</summary>
    public void RemoveTag(TagId tagId)
    {
        var tag = _tags.FirstOrDefault(t => t.TagId == tagId)
            ?? throw new PostTagNotAssignedException(Id, tagId);

        _tags.Remove(tag);
        RaiseDomainEvent(new PostTagRemoved(Id, tagId));
    }
}
