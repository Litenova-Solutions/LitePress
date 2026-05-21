# Read Model Inventory

<!-- Last updated: 2026-05-21 -->

This file gives engineers and agents a map of the read side. Query handlers inject
`IDatabaseContext` and write LINQ projections directly. There are no per-aggregate
`IXxxReadStore` interfaces.

Update this file in the same PR that adds a new query handler, a new
`IDatabaseContext` property, or an approved denormalized read model.

See `standards/docs/conventions/backend/07-query-read-strategy.md` for the full
`IDatabaseContext` projection pattern.

---

## IDatabaseContext Properties

`IDatabaseContext` is defined in `Blog.Application.Read.Contracts/Shared/IDatabaseContext.cs`.

| Aggregate | Property | Notes |
|:---|:---|:---|
| `Post` | `IQueryable<Post> Posts` | Used by all post query handlers. Always filter by `State` before projecting. |
| `Tag` | `IQueryable<Tag> Tags` | Used by tag list and post-by-tag query handlers. |
| `Author` | `IQueryable<Author> Authors` | Used when projecting Author display name onto post results. |

---

## Query Handlers

All query handlers live in `Blog.Application.Read`. They inject `IDatabaseContext`
and project directly — never inject repositories or `AppDbContext`.

| Query | Handler | Result Type | Reads From |
|:---|:---|:---|:---|
| `GetPostByIdQuery` | `GetPostByIdQueryHandler` | `PostDetailResult` | `Posts`, `Authors`, `Tags` |
| `GetPostBySlugQuery` | `GetPostBySlugQueryHandler` | `PostDetailResult` | `Posts`, `Authors`, `Tags` |
| `GetAllPostsQuery` | `GetAllPostsQueryHandler` | `PagedResult<PostSummaryResult>` | `Posts`, `Authors`, `Tags` |
| `GetPublishedPostsQuery` | `GetPublishedPostsQueryHandler` | `PagedResult<PostSummaryResult>` | `Posts` (state=Published), `Authors`, `Tags` |
| `GetPostsByTagQuery` | `GetPostsByTagQueryHandler` | `PagedResult<PostSummaryResult>` | `Posts` (state=Published), `Tags` |
| `GetAllTagsQuery` | `GetAllTagsQueryHandler` | `IReadOnlyList<TagResult>` | `Tags`, `Posts` (for post count) |
| `GetTagBySlugQuery` | `GetTagBySlugQueryHandler` | `TagResult` | `Tags` |
| `GetAuthorByIdQuery` | `GetAuthorByIdQueryHandler` | `AuthorResult` | `Authors` |

---

## Result Record Shapes

### PostSummaryResult
```csharp
// Blog.Application.Read.Contracts/Posts/Queries/GetPublishedPosts/PostSummaryResult.cs
record PostSummaryResult(
    Guid PostId,
    string Title,
    string Slug,
    string? Excerpt,
    string? CoverImageUrl,
    string AuthorDisplayName,
    DateTimeOffset? PublishedAt,
    IReadOnlyList<TagSummaryResult> Tags
);
```

### PostDetailResult
```csharp
// Blog.Application.Read.Contracts/Posts/Queries/GetPostBySlug/PostDetailResult.cs
record PostDetailResult(
    Guid PostId,
    string Title,
    string Slug,
    string Content,         // ProseMirror JSON string
    string? Excerpt,
    string? CoverImageUrl,
    string AuthorDisplayName,
    string PostState,
    DateTimeOffset CreatedAt,
    DateTimeOffset? PublishedAt,
    IReadOnlyList<TagSummaryResult> Tags
);
```

### TagResult
```csharp
// Blog.Application.Read.Contracts/Tags/Queries/GetAllTags/TagResult.cs
record TagResult(
    Guid TagId,
    string Name,
    string Slug,
    int PostCount
);
```

### TagSummaryResult
```csharp
// Blog.Application.Read.Contracts/Shared/TagSummaryResult.cs
record TagSummaryResult(
    Guid TagId,
    string Name,
    string Slug
);
```

---

## Pagination Convention

All paged queries use `PagedResult<T>` from `Blog.Application.Read.Contracts`.

```csharp
record PagedResult<T>(
    IReadOnlyList<T> Items,
    int Page,
    int PageSize,
    int TotalCount
);
```

Default page size: **10**. Maximum page size: **50**.

See `standards/docs/adr/0017-pagination-convention.md` for the full offset pagination convention.

---

## Denormalized Read Models

No denormalized read model tables are approved for v1. All queries use
`IDatabaseContext` LINQ projections over the domain tables.

| Read Model | Owner | Update Mechanism | Reconciliation Job | ADR |
|:---|:---|:---|:---|:---|
| _(none for v1)_ | — | — | — | — |
