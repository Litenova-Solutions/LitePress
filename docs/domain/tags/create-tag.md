# Create Tag

| Field | Value |
|:---|:---|
| Feature | `tags` |
| Status | Active (backend complete; admin UI partial) |
| Last updated | 2026-05-23 |

---

## Summary

An authenticated Author creates a new Tag with a unique name. Slug is generated automatically.

---

## Command

| Type | Name | Input | Output |
|:---|:---|:---|:---|
| Command | `CreateTagCommand` | `TagId`, `Name` | `CreateTagCommandResult` (`TagId`, `Slug`) |

### Structural validation

- Name: required, max 50 characters

---

## Domain Behavior

- Creates `Tag` aggregate.
- Raises `TagCreated`.

---

## Exceptions

| Exception | When | HTTP status |
|:---|:---:|---:|
| `TagNameRequiredException` | Empty name | 400 |
| `TagNameTooLongException` | Name > 50 chars | 400 |
| `TagNameAlreadyExistsException` | Duplicate name | 409 |

---

## HTTP Endpoint

| Method | Path | Auth |
|:---|:---|:---|
| POST | `/api/tags` | Bearer JWT |

Request: `{ name }`. Returns 201 with `{ tagId, slug }`.

---

## UI (admin)

Inline create form on tags management page.

---

## Acceptance Criteria

1. Given a unique name, when created, then tag appears in tag list with correct slug. (Integration)
2. Given a duplicate name (case-insensitive), when created, then API returns 409. (Integration)

---

## Out of Scope

Assigning tag to posts (see add-tag-to-post).
