# Aggregate Inventory

<!-- Last updated: 2026-05-21 -->

This file gives engineers and agents a single place to find all aggregates, their
state machines, and their domain events. Before generating a command handler or a
repository implementation, check this file to confirm the aggregate's current states
and which events it raises.

Update this table in the same PR that adds a new aggregate, changes its states,
or introduces new domain events.

---

## Aggregates

| Aggregate | ID Type | States | Domain Events | Repository Interface |
|:---|:---|:---|:---|:---|
| `Post` | `PostId` | `DraftPostState`, `PublishedPostState`, `ArchivedPostState` | `PostCreated`, `PostUpdated`, `PostPublished`, `PostArchived`, `PostDeleted`, `PostTagAdded`, `PostTagRemoved` | `IPostRepository` |
| `Tag` | `TagId` | _(stateless — active by existence)_ | `TagCreated`, `TagRenamed`, `TagDeleted` | `ITagRepository` |
| `Author` | `AuthorId` | `ActiveAuthorState` | `AuthorRegistered` | `IAuthorRepository` |

---

## Post State Machine

```
[Created]
    │
    ▼
 DraftPostState  ──── Publish() ────►  PublishedPostState
       │                                       │
       │                                  Archive()
       │                                       │
       └────────── Archive() ────────►  ArchivedPostState
```

Rules:
- A `Post` is created in `DraftPostState`.
- `Publish()` transitions `DraftPostState` → `PublishedPostState`. Throws `PostAlreadyPublishedException` if already published.
- `Archive()` transitions any non-archived state → `ArchivedPostState`. Throws `PostAlreadyArchivedException` if already archived.
- An archived Post cannot be re-published. A new Post must be created instead.
- `Update()` (title, content, excerpt, cover image, tags) is allowed in `DraftPostState` only. Throws `PostNotEditableException` if in any other state.
- `Slug` is generated from the title on creation and becomes immutable once the Post is published.

---

## Value Objects

| Value Object | Used By | Rules |
|:---|:---|:---|
| `PostId` | `Post` | Strongly typed ID, `Guid` backing value. |
| `PostTitle` | `Post` | Required. 1–200 characters. |
| `PostSlug` | `Post` | URL-safe, lowercase, hyphen-separated. Derived from `PostTitle`. Immutable once Published. |
| `PostContent` | `Post` | Required. Stored as JSON (ProseMirror/TipTap document format). |
| `PostExcerpt` | `Post` | Optional. Plain text. Max 500 characters. Auto-derived from first paragraph if not provided. |
| `PostCoverImageUrl` | `Post` | Optional. Absolute URL string. Must be a valid URL. |
| `TagId` | `Tag` | Strongly typed ID, `Guid` backing value. |
| `TagName` | `Tag` | Required. 1–50 characters. Case-insensitive unique across all Tags. |
| `TagSlug` | `Tag` | URL-safe, lowercase, hyphen-separated. Derived from `TagName`. Immutable. |
| `AuthorId` | `Author`, `Post` | Strongly typed ID, `Guid` backing value. Sourced from JWT claim only. |

---

## Domain Shared Types

| Type | Purpose |
|:---|:---|
| `AggregateRoot<TId>` | Base class for all aggregate roots. Provides domain event collection. Defined in `Domain/Shared/AggregateRoot.cs`. |
| `IDomainEvent` | Marker interface for all domain events. Defined in `Domain/Shared/IDomainEvent.cs`. |
| `DomainException` | Abstract base class for all domain invariant violation exceptions. |
| `AggregateNotFoundException` | Abstract base class for all not-found exceptions. |

---

## Domain Events per Aggregate

### Post

| Event | Raised By | Payload |
|:---|:---|:---|
| `PostCreated` | `Post` constructor | `PostId`, `AuthorId`, `PostTitle`, `PostSlug`, `PostContent`, `PostExcerpt?`, `PostCoverImageUrl?`, `IReadOnlyList<TagId>` |
| `PostUpdated` | `Post.Update()` | `PostId`, `PostTitle`, `PostSlug`, `PostContent`, `PostExcerpt?`, `PostCoverImageUrl?` |
| `PostPublished` | `Post.Publish()` | `PostId`, `AuthorId`, `PublishedAt` |
| `PostArchived` | `Post.Archive()` | `PostId`, `ArchivedAt` |
| `PostDeleted` | `Post.Delete()` | `PostId` |
| `PostTagAdded` | `Post.AddTag()` | `PostId`, `TagId` |
| `PostTagRemoved` | `Post.RemoveTag()` | `PostId`, `TagId` |

### Tag

| Event | Raised By | Payload |
|:---|:---|:---|
| `TagCreated` | `Tag` constructor | `TagId`, `TagName`, `TagSlug` |
| `TagRenamed` | `Tag.Rename()` | `TagId`, `NewTagName`, `NewTagSlug` |
| `TagDeleted` | `Tag.Delete()` | `TagId` |

### Author

| Event | Raised By | Payload |
|:---|:---|:---|
| `AuthorRegistered` | `Author` constructor | `AuthorId`, `DisplayName` |
