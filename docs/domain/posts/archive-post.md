# Archive Post

| Field | Value |
|:---|:---|
| Feature | `posts` |
| Status | Active (backend complete; admin UI partial) |
| Last updated | 2026-05-23 |

---

## Summary

An authenticated Author archives a post, removing it from public view without deleting it. Works from Draft or Published state.

---

## Command

| Type | Name | Input | Output | Idempotency |
|:---|:---|:---|:---|:---:|
| Command | `ArchivePostCommand` | `PostId` | `ArchivePostCommandResult` | No |

---

## Domain Behavior

- `Post.Archive()` transitions to Archived.
- Sets `ArchivedAt` to UTC now.
- Raises `PostArchived`.

---

## Exceptions

| Exception | When | HTTP status |
|:---|:---:|---:|
| `PostNotFoundException` | Post ID not found | 404 |
| `PostAlreadyArchivedException` | Already archived | 409 |

---

## HTTP Endpoint

| Method | Path | Auth |
|:---|:---|:---|
| POST | `/api/posts/{id}/archive` | Bearer JWT |

Returns 204 No Content.

---

## UI (admin)

Archive action on post edit page and post list row actions.

---

## Acceptance Criteria

1. Given a Published post, when archived, then it no longer appears in public post listings. (Integration)
2. Given an Archived post, when archive is called again, then API returns 409. (Integration)

---

## Out of Scope

Re-publishing archived posts (forbidden by domain rules).
