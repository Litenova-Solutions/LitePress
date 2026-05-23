# Add Tag to Post

| Field | Value |
|:---|:---|
| Feature | `posts` |
| Status | Active (v1 complete) |
| Last updated | 2026-05-23 |

---

## Summary

An authenticated Author assigns an existing Tag to a Post. Tags can also be assigned at creation time via `CreatePostCommand.TagIds`. This use case covers adding or removing tags on an existing Draft post.

---

## Commands

| Type | Name | Input | Output |
|:---|:---|:---|:---|
| Command | `AddTagToPostCommand` | `PostId`, `TagId` | `AddTagToPostCommandResult` |
| Command | `RemoveTagFromPostCommand` | `PostId`, `TagId` | `RemoveTagFromPostCommandResult` |

---

## Domain Behavior

- `Post.AddTag(tagId)`: max 10 tags, no duplicates.
- `Post.RemoveTag(tagId)`: tag must be assigned.
- Raises `PostTagAdded` / `PostTagRemoved`.

---

## Exceptions

| Exception | When | HTTP status |
|:---|:---:|---:|
| `PostNotFoundException` | Post not found | 404 |
| `TagNotFoundException` | Tag not found | 404 |
| `PostTagLimitExceededException` | Already 10 tags | 422 |
| `PostTagAlreadyAssignedException` | Duplicate tag | 409 |
| `PostTagNotAssignedException` | Remove unassigned tag | 409 |
| `PostNotEditableException` | Post not in Draft | 409 |

---

## HTTP Endpoints

| Method | Path | Auth |
|:---|:---|:---|
| POST | `/api/posts/{id}/tags` | Bearer JWT |
| DELETE | `/api/posts/{id}/tags/{tagId}` | Bearer JWT |

Request body for add: `{ tagId }`.

---

## UI (admin)

Tag multi-select on post create/edit form. Shows all tags with checkboxes or combobox.

---

## Acceptance Criteria

1. Given a Draft post with fewer than 10 tags, when a valid tag is added, then the tag appears on the post detail and public listing after publish. (Integration + Playwright)
2. Given 10 tags already assigned, when another tag is added, then API returns 422. (Integration)
3. Given a duplicate tag, when add is attempted, then API returns 409. (Integration)

---

## Out of Scope

Creating tags inline (use create-tag use case). Tag assignment on Published posts.
