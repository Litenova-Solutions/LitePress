# Register Author

| Field | Value |
|:---|:---|
| Feature | `authors` |
| Status | Active |
| Last updated | 2026-05-23 |

---

## Summary

When an allowed GitHub user first accesses a mutating API endpoint, the system ensures an Author record exists. Registration is automatic and idempotent via `EnsureAuthorMiddleware` and the `RegisterAuthorCommand`.

---

## Command

| Type | Name | Input | Output |
|:---|:---|:---|:---|
| Command | `RegisterAuthorCommand` | `AuthorId`, `DisplayName` | `RegisterAuthorCommandResult` |

`AuthorId` is derived from JWT `sub` claim, never from request body.

---

## Domain Behavior

- Creates `Author` in `ActiveAuthorState`.
- Raises `AuthorRegistered`.
- If author already exists, returns existing record without error.

---

## Exceptions

| Exception | When | HTTP status |
|:---|:---:|---:|
| `AuthorNotFoundException` | Query by ID when not registered | 404 |

---

## HTTP Surface

No dedicated public endpoint. Triggered by:

- `EnsureAuthorMiddleware` on authenticated API requests
- Implicit on first admin API call after OAuth login

Admin OAuth gate: only GitHub user matching `GITHUB_OWNER_ID` may sign in. See [docs/decisions/admin-auth.md](../../decisions/admin-auth.md).

---

## UI projection

| App | Page doc | Role on page |
|:---|:---|:---|
| admin | [login.md](../../ui/admin/pages/login.md) | OAuth sign-in triggers first API call after redirect |

No dedicated registration UI. Author is registered transparently on first authenticated API interaction after GitHub OAuth login.

---

## Acceptance Criteria

1. Given a new GitHub owner login, when they create a post, then an Author row exists with matching `AuthorId`. (Integration)
2. Given an existing Author, when middleware runs again, then no duplicate row is created. (Integration)
3. Given a non-owner GitHub account, when sign-in is attempted, then Auth.js rejects the login. (Manual)

---

## Out of Scope

Multi-author permissions. Author profile editing.
