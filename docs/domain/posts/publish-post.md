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

## UI projection

| App | Page doc | Role on page |
|:---|:---|:---|
| admin | [post-editor.md](../../ui/admin/pages/post-editor.md) | Publish action on shared editor |

Shell: [admin shell.md](../../ui/admin/shell.md)

### Operation states

| State | Behavior |
|:---|:---|
| Loading | Publish button disabled during action |
| Error | Toast with conflict message if not Draft |
| Loaded | Post status badge shows Published; edit form disabled |

---

## Acceptance Criteria

| ID | Criterion | Test type |
|:---|:---|:---|
| AC-001 | Given a Draft post, when published, then it appears in `GET /api/posts` for anonymous callers. | BDD acceptance (`PublishPost.feature` @ac:AC-001) |
| AC-002 | Given a Draft post, when published via admin, then the public home page lists it within cache revalidation window. | Playwright |
| AC-003 | Given a Published post, when publish is called again, then API returns 409. | BDD acceptance (`PublishPost.feature` @ac:AC-003) |
| AC-004 | Given a publish request without authentication, when publish is called, then API returns 401. | BDD acceptance (`PublishPost.feature` @ac:AC-004) |

---

## Acceptance Coverage

| ID | Criterion summary | Risk | Required test type | BDD scenario | Plain API test | Domain/Application test | Manual only |
|:---|:---|:---|:---|:---|:---|:---|:---:|
| AC-001 | Draft publish visible on public feed | Critical | BDD acceptance | Author publishes a draft post | | `PostTests.Publish_WhenPostIsDraft_*` | |
| AC-002 | Public home lists post after admin publish | High | Playwright | | | | |
| AC-003 | Re-publish returns 409 | Critical | BDD acceptance | Publishing an already published post is rejected | | `PostTests.Publish_WhenPostIsAlreadyPublished_*` | |
| AC-004 | Unauthenticated publish returns 401 | Critical | BDD acceptance | Unauthenticated publish request is rejected | | | |

**BDD decision:** BDD acceptance for AC-001, AC-003, and AC-004 (business-visible publish rules and auth). Playwright covers AC-002. Domain tests cover aggregate transitions.

---

## Out of Scope

Scheduled publishing.
