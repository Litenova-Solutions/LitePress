# Create Post

| Field | Value |
|:---|:---|
| Feature | `posts` |
| Status | Active (v1 complete) |
| Last updated | 2026-05-25 |

---

## Summary

An authenticated Author creates a new Post in Draft state with title, ProseMirror JSON content, optional excerpt and cover image URL, and optional initial tag assignments. The API returns the new `PostId` and generated slug.

---

## Command

| Type | Name | Input | Output | Idempotency |
|:---|:---|:---|:---|:---:|
| Command | `CreatePostCommand` | `PostId`, `AuthorId` (from JWT), `Title`, `Content`, `Excerpt?`, `CoverImageUrl?`, `TagIds[]` | `CreatePostCommandResult` (`PostId`, `Slug`) | No |

### Structural validation

- Title: required, max 200 characters
- Content: required (ProseMirror JSON string)
- Excerpt: optional, max 500 characters

---

## Domain Behavior

- Factory `Post.Create()` sets Draft state, generates slug from title, assigns tags from `TagIds`.
- Raises `PostCreated`.

---

## Exceptions

| Exception | When | HTTP status |
|:---|:---:|---:|
| `PostTitleRequiredException` | Empty title | 400 |
| `PostTitleTooLongException` | Title > 200 chars | 400 |
| `PostContentRequiredException` | Empty content | 400 |
| `PostExcerptTooLongException` | Excerpt > 500 chars | 400 |
| `TagNotFoundException` | Invalid tag ID in `TagIds` | 404 |

---

## HTTP Endpoint

| Method | Path | Auth | Rate limit |
|:---|:---|:---|:---|
| POST | `/api/posts` | Bearer JWT | authenticated-api |

Request body: `{ title, content, excerpt?, coverImageUrl?, tagIds[] }`.

Returns 201 with `{ postId, slug }` and `Location: /api/posts/{postId}`.

---

## UI projection

| App | Page doc | Role on page |
|:---|:---|:---|
| admin | [post-create.md](../../ui/admin/pages/post-create.md) | Create form |

Shell: [admin shell.md](../../ui/admin/shell.md)

### Operation states

| State | Behavior |
|:---|:---|
| Submitting | Submit button disabled during client mutation |
| Empty | Blank form with TipTap editor |
| Error | Inline alert and error toast on API failure |
| Success | Redirect to post edit page |

### Mutations

Client mutation via `/api-proxy/posts` with JSON body. TipTap outputs ProseMirror JSON for `content`.

---

## Acceptance Criteria

1. Given an authenticated Author, when they submit a valid title and content, then a Draft post is created and they are redirected to the edit page. (Playwright)
2. Given an empty title, when they submit, then validation fails before the API call. (Vitest)
3. Given valid input, when the API receives the command, then a Draft post exists with the correct `AuthorId`. (Domain + Integration)

---

## Out of Scope

Publishing, scheduled publishing, cover image upload to R2.
