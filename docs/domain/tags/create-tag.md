# Create Tag

| Field | Value |
|:---|:---|
| Feature | `tags` |
| Status | Active (v1 complete) |
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

## UI projection

| App | Page doc | Role on page |
|:---|:---|:---|
| admin | [tags.md](../../ui/admin/pages/tags.md) | Inline create form |

Shell: [admin shell.md](../../ui/admin/shell.md)

---

## Acceptance Criteria

| ID | Criterion | Test type |
|:---|:---|:---|
| AC-001 | Given a unique name, when created, then tag appears in tag list with correct slug. | BDD acceptance (`CreateTag.feature` @ac:AC-001) |
| AC-002 | Given a duplicate name, when created, then API returns 409. | BDD acceptance (`CreateTag.feature` @ac:AC-002) |

---

## Acceptance Coverage

| ID | Criterion summary | Risk | Required test type | BDD scenario | Plain API test | Domain/Application test | Manual only |
|:---|:---|:---|:---|:---|:---|:---|:---:|
| AC-001 | Unique tag visible in list | Critical | BDD acceptance | Author creates a unique tag | | | |
| AC-002 | Duplicate name returns 409 | Critical | BDD acceptance | Duplicate tag name is rejected | | | |

**BDD decision:** BDD acceptance for create rules visible to authors.

---

## Out of Scope

Assigning tag to posts (see add-tag-to-post).
