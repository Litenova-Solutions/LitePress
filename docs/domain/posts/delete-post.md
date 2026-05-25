# Delete Post

| Field | Value |
|:---|:---|
| Feature | `posts` |
| Status | Active (v1 complete) |
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

## UI projection

| App | Page doc | Role on page |
|:---|:---|:---|
| admin | [post-editor.md](../../ui/admin/pages/post-editor.md) | Delete action with confirmation dialog |

Shell: [admin shell.md](../../ui/admin/shell.md)

---

## Acceptance Criteria

1. Given a Draft post, when deleted, then `GET /api/posts/{id}` returns 404. (Integration)
2. Given a Published post, when delete is attempted, then API returns 409. (Integration)

---

## Out of Scope

Soft-delete of published posts (use archive instead).
