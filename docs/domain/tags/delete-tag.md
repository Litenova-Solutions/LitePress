# Delete Tag

| Field | Value |
|:---|:---|
| Feature | `tags` |
| Status | Active (v1 complete) |
| Last updated | 2026-05-23 |

---

## Summary

An authenticated Author deletes a Tag. The tag is removed from all Posts that reference it before deletion.

---

## Command

| Type | Name | Input | Output |
|:---|:---|:---|:---|
| Command | `DeleteTagCommand` | `TagId` | `DeleteTagCommandResult` |

---

## Domain Behavior

- Handler removes tag from all posts, then deletes tag aggregate.
- Raises `TagDeleted`.

---

## Exceptions

| Exception | When | HTTP status |
|:---|:---:|---:|
| `TagNotFoundException` | Tag not found | 404 |

---

## HTTP Endpoint

| Method | Path | Auth |
|:---|:---|:---|
| DELETE | `/api/tags/{id}` | Bearer JWT |

Returns 204 No Content.

---

## Acceptance Criteria

1. Given a tag assigned to posts, when deleted, then posts no longer reference that tag. (Integration)
2. Given a deleted tag slug, when visiting `/tags/{slug}`, then 404 or empty list. (Playwright)

---

## Out of Scope

Archive tag (tags have no archived state).
