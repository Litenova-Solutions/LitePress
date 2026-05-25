# Login (`/login`)

| Field | Value |
|:---|:---|
| App | `apps/admin` |
| Route | `/login` |
| Route shell | [app/(auth)/login/page.tsx](../../../apps/admin/app/(auth)/login/page.tsx) |
| Feature entry | (none — Auth.js) |
| Last updated | 2026-05-25 |

---

## Use cases on this page

GitHub OAuth via Auth.js. No domain command. After sign-in, [register-author.md](../../domain/authors/register-author.md) runs on first API request.

---

## Screen states

| State | User sees |
|:---|:---|
| Unauthenticated | GitHub sign-in button |
| Authenticated | Redirect to `/` (dashboard) |

---

## Shell

Inherits [admin shell](../shell.md) auth layout only. No dashboard chrome.

---

## Tests

| Type | Location |
|:---|:---|
| Playwright | Not yet added for admin |
| Manual | [admin-auth ADR](../../decisions/admin-auth.md) — owner-only GitHub login |

Acceptance criteria: [register-author.md § Acceptance Criteria](../../domain/authors/register-author.md#acceptance-criteria).

---

## Related

[admin-auth ADR](../../decisions/admin-auth.md)
