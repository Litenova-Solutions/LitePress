# Update Post

| Field | Value |
|:---|:---|
| Feature | `posts` |
| Status | Active (v1 complete) |
| Last updated | 2026-05-23 |

---

## Summary

An authenticated Author updates title, content, excerpt, and cover image URL on a Draft post. Slug is regenerated from the new title while the post remains in Draft.

---

## Command

| Type | Name | Input | Output | Idempotency |
|:---|:---|:---|:---|:---:|
| Command | `UpdatePostCommand` | `PostId`, `Title`, `Content`, `Excerpt?`, `CoverImageUrl?` | `UpdatePostCommandResult` (`PostId`, `Slug`) | No |

### Structural validation

Same rules as create post (title, content, excerpt).

---

## Domain Behavior

- `Post.Update()` allowed in Draft only.
- Regenerates slug from title.
- Raises `PostUpdated`.

---

## Exceptions

| Exception | When | HTTP status |
|:---|:---:|---:|
| `PostNotFoundException` | Post ID not found | 404 |
| `PostNotEditableException` | Post not in Draft | 409 |
| `PostTitleRequiredException` | Empty title | 400 |
| `PostTitleTooLongException` | Title > 200 chars | 400 |
| `PostContentRequiredException` | Empty content | 400 |
| `PostExcerptTooLongException` | Excerpt > 500 chars | 400 |

---

## HTTP Endpoint

| Method | Path | Auth |
|:---|:---|:---|
| PUT | `/api/posts/{id}` | Bearer JWT |

Request body: `{ title, content, excerpt?, coverImageUrl? }`.

Returns 200 with `{ postId, slug }`.

---

## UI projection

| App | Page doc | Role on page |
|:---|:---|:---|
| admin | [post-editor.md](../../ui/admin/pages/post-editor.md) | Edit form (Draft only) |

Shell: [admin shell.md](../../ui/admin/shell.md)

Tag assignment is a separate use case ([add-tag-to-post.md](add-tag-to-post.md)).

### Operation states

| State | Behavior |
|:---|:---|
| Loading | Skeleton while fetching post by ID |
| Empty | N/A (404 if post missing) |
| Error | Toast on save failure; inline validation |
| Loaded | Form pre-filled; save enabled only in Draft |

---

## Acceptance Criteria

1. Given a Draft post, when the Author saves valid changes, then the post content and slug update. (Integration)
2. Given a Published post, when the Author attempts to save, then the API returns 409. (Integration)
3. Given valid edits, when saved via admin UI, then changes persist on reload. (Playwright)

---

## Out of Scope

Updating tags (separate use case). Editing published posts.
