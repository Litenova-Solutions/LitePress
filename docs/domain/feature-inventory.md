# Feature Inventory

<!-- Last updated: 2026-05-21 -->

This file gives agents a map of all use cases before they generate new handlers.
Before adding a new command or query handler, check this file to avoid duplicates.
Update this table in the same PR that adds or removes a handler.

---

## Use Cases

### Posts

| Feature | Use Case | Type | Handler Class | Status |
|:---|:---|:---|:---|:---|
| Posts | Create Post | Command | `CreatePostCommandHandler` | Planned |
| Posts | Update Post | Command | `UpdatePostCommandHandler` | Planned |
| Posts | Publish Post | Command | `PublishPostCommandHandler` | Planned |
| Posts | Archive Post | Command | `ArchivePostCommandHandler` | Planned |
| Posts | Delete Post | Command | `DeletePostCommandHandler` | Planned |
| Posts | Add Tag to Post | Command | `AddTagToPostCommandHandler` | Planned |
| Posts | Remove Tag from Post | Command | `RemoveTagFromPostCommandHandler` | Planned |
| Posts | Get Post by ID | Query | `GetPostByIdQueryHandler` | Planned |
| Posts | Get Post by Slug | Query | `GetPostBySlugQueryHandler` | Planned |
| Posts | Get All Posts (paged, admin) | Query | `GetAllPostsQueryHandler` | Planned |
| Posts | Get Published Posts (paged, public) | Query | `GetPublishedPostsQueryHandler` | Planned |
| Posts | Get Posts by Tag (paged, public) | Query | `GetPostsByTagQueryHandler` | Planned |

### Tags

| Feature | Use Case | Type | Handler Class | Status |
|:---|:---|:---|:---|:---|
| Tags | Create Tag | Command | `CreateTagCommandHandler` | Planned |
| Tags | Rename Tag | Command | `RenameTagCommandHandler` | Planned |
| Tags | Delete Tag | Command | `DeleteTagCommandHandler` | Planned |
| Tags | Get All Tags | Query | `GetAllTagsQueryHandler` | Planned |
| Tags | Get Tag by Slug | Query | `GetTagBySlugQueryHandler` | Planned |

### Authors

| Feature | Use Case | Type | Handler Class | Status |
|:---|:---|:---|:---|:---|
| Authors | Register Author | Command | `RegisterAuthorCommandHandler` | Planned |
| Authors | Get Author by ID | Query | `GetAuthorByIdQueryHandler` | Planned |

---

## Command Details

### CreatePostCommand
- **Contracts project:** `Blog.Application.Write.Contracts`
- **Handler project:** `Blog.Application.Write`
- **Input:** `AuthorId` (from JWT), `Title`, `Content`, `Excerpt?`, `CoverImageUrl?`, `TagIds[]`
- **Result:** `CreatePostCommandResult` containing the new `PostId` and `Slug`
- **Raises:** `PostCreated`

### UpdatePostCommand
- **Contracts project:** `Blog.Application.Write.Contracts`
- **Handler project:** `Blog.Application.Write`
- **Input:** `PostId`, `Title`, `Content`, `Excerpt?`, `CoverImageUrl?`
- **Result:** `UpdatePostCommandResult`
- **Raises:** `PostUpdated`
- **Guard:** Post must be in `DraftPostState`. Throws `PostNotEditableException` otherwise.

### PublishPostCommand
- **Contracts project:** `Blog.Application.Write.Contracts`
- **Handler project:** `Blog.Application.Write`
- **Input:** `PostId`
- **Result:** `PublishPostCommandResult`
- **Raises:** `PostPublished`
- **Guard:** Post must be in `DraftPostState`. Throws `PostAlreadyPublishedException` otherwise.

### ArchivePostCommand
- **Contracts project:** `Blog.Application.Write.Contracts`
- **Handler project:** `Blog.Application.Write`
- **Input:** `PostId`
- **Result:** `ArchivePostCommandResult`
- **Raises:** `PostArchived`
- **Guard:** Post must not already be `ArchivedPostState`. Throws `PostAlreadyArchivedException` otherwise.

### DeletePostCommand
- **Contracts project:** `Blog.Application.Write.Contracts`
- **Handler project:** `Blog.Application.Write`
- **Input:** `PostId`
- **Result:** `DeletePostCommandResult`
- **Raises:** `PostDeleted`
- **Guard:** Post must be in `DraftPostState` or `ArchivedPostState`. Cannot delete a Published post directly.

### CreateTagCommand
- **Contracts project:** `Blog.Application.Write.Contracts`
- **Handler project:** `Blog.Application.Write`
- **Input:** `Name`
- **Result:** `CreateTagCommandResult` containing `TagId` and `Slug`
- **Raises:** `TagCreated`
- **Guard:** Tag name must be unique (case-insensitive). Throws `TagNameAlreadyExistsException`.

### RenameTagCommand
- **Contracts project:** `Blog.Application.Write.Contracts`
- **Handler project:** `Blog.Application.Write`
- **Input:** `TagId`, `NewName`
- **Result:** `RenameTagCommandResult`
- **Raises:** `TagRenamed`
- **Guard:** New name must be unique. Throws `TagNameAlreadyExistsException`.

### DeleteTagCommand
- **Contracts project:** `Blog.Application.Write.Contracts`
- **Handler project:** `Blog.Application.Write`
- **Input:** `TagId`
- **Result:** `DeleteTagCommandResult`
- **Raises:** `TagDeleted`
- **Side effect:** Removes the Tag from all Posts that reference it.

---

## Query Details

### GetAllPostsQuery (admin)
- Returns all posts regardless of state, ordered by `CreatedAt` descending.
- Supports pagination via `Page` and `PageSize`.
- Returns `PagedResult<PostSummaryResult>`.

### GetPublishedPostsQuery (public)
- Returns only `PublishedPostState` posts, ordered by `PublishedAt` descending.
- Supports pagination via `Page` and `PageSize`.
- Returns `PagedResult<PostSummaryResult>`.

### GetPostsByTagQuery (public)
- Filters published posts by a given `TagSlug`.
- Supports pagination.
- Returns `PagedResult<PostSummaryResult>`.

### GetPostBySlugQuery (public)
- Returns a single published post by `PostSlug`.
- Returns `PostDetailResult`.
- Returns 404 if not found or not published.

### GetAllTagsQuery
- Returns all tags with their post counts.
- Returns `IReadOnlyList<TagResult>`.
- No pagination (tag count is bounded by design).
