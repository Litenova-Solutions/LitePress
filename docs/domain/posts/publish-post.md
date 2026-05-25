# Publish Post

| Field | Value |
|:---|:---|
| Feature | `posts` |
| Status | Active (v1 complete) |
| Last updated | 2026-05-23 |

---

## Summary

An authenticated Author publishes a Draft post, making it visible on the public web. The slug becomes immutable and `PublishedAt` is set.

---

## Command

| Type | Name | Input | Output | Idempotency |
|:---|:---|:---|:---|:---:|
| Command | `PublishPostCommand` | `PostId` | `PublishPostCommandResult` | No |

---

## Domain Behavior

- `Post.Publish()` transitions Draft → Published.
- Sets `PublishedAt` to UTC now.
- Raises `PostPublished`.

---

## Exceptions

| Exception | When | HTTP status |
|:---|:---:|---:|
| `PostNotFoundException` | Post ID not found | 404 |
| `PostAlreadyPublishedException` | Already published | 409 |
| `PostNotEditableException` | Not in Draft (e.g. Archived) | 409 |

---

## HTTP Endpoint

| Method | Path | Auth |
|:---|:---|:---|
| POST | `/api/posts/{id}/publish` | Bearer JWT |

Returns 204 No Content.

---

## UI (admin)

Page composition: [docs/ui/admin/pages/post-editor.md](../../ui/admin/pages/post-editor.md) (shared editor surface).

### States

| State | Behavior |
|:---|:---|
| Loading | Publish button disabled during action |
| Error | Toast with conflict message if not Draft |
| Loaded | Post status badge shows Published; edit form disabled |

---

## Acceptance Criteria

1. Given a Draft post, when published, then it appears in `GET /api/posts` for anonymous callers. (Integration)
2. Given a Draft post, when published via admin, then the public home page lists it within cache revalidation window. (Playwright)
3. Given a Published post, when publish is called again, then API returns 409. (Integration)

---

## Out of Scope

Scheduled publishing.
