# Rename Tag

| Field | Value |
|:---|:---|
| Feature | `tags` |
| Status | Active (v1 complete) |
| Last updated | 2026-05-23 |

---

## Summary

An authenticated Author renames an existing Tag. Slug is regenerated from the new name.

---

## Command

| Type | Name | Input | Output |
|:---|:---|:---|:---|
| Command | `RenameTagCommand` | `TagId`, `NewName` | `RenameTagCommandResult` |

---

## Domain Behavior

- `Tag.Rename(newName)` updates name and slug.
- Raises `TagRenamed`.

---

## Exceptions

| Exception | When | HTTP status |
|:---|:---:|---:|
| `TagNotFoundException` | Tag not found | 404 |
| `TagNameRequiredException` | Empty name | 400 |
| `TagNameTooLongException` | Name > 50 chars | 400 |
| `TagNameAlreadyExistsException` | Duplicate name | 409 |

---

## HTTP Endpoint

| Method | Path | Auth |
|:---|:---|:---|
| PUT | `/api/tags/{id}` | Bearer JWT |

Request: `{ name }`. Returns 200.

---

## Acceptance Criteria

1. Given an existing tag, when renamed to a unique name, then slug updates and public tag URLs reflect the new slug after cache revalidation. (Integration)

---

## Out of Scope

Bulk rename. Tag merge.
