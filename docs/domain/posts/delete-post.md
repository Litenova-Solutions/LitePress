# Delete Post

| Field | Value |
|:---|:---|
| Feature | `posts` |
| Status | Active (backend complete; admin UI partial) |
| Last updated | 2026-05-23 |

---

## Summary

An authenticated Author permanently deletes a Draft or Archived post. Published posts cannot be deleted directly; they must be archived first.

---

## Command

| Type | Name | Input | Output | Idempotency |
|:---|:---|:---|:---|:---:|
| Command | `DeletePostCommand` | `PostId` | `DeletePostCommandResult` | No |

---

## Domain Behavior

- `Post.Delete()` raises `PostDeleted`.
- Forbidden when Published. Throws `PostCannotBeDeletedException`.

---

## Exceptions

| Exception | When | HTTP status |
|:---|:---:|---:|
| `PostNotFoundException` | Post ID not found | 404 |
| `PostCannotBeDeletedException` | Post is Published | 409 |

---

## HTTP Endpoint

| Method | Path | Auth |
|:---|:---|:---|
| DELETE | `/api/posts/{id}` | Bearer JWT |

Returns 204 No Content.

---

## UI (admin)

Delete action with confirmation dialog on post edit page.

---

## Acceptance Criteria

1. Given a Draft post, when deleted, then `GET /api/posts/{id}` returns 404. (Integration)
2. Given a Published post, when delete is attempted, then API returns 409. (Integration)

---

## Out of Scope

Soft-delete of published posts (use archive instead).
