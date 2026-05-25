# Dashboard (`/`)

| Field | Value |
|:---|:---|
| App | `apps/admin` |
| Route | `/` |
| Route shell | [app/(dashboard)/page.tsx](../../../apps/admin/app/(dashboard)/page.tsx) |
| Feature entry | (inline in route shell) |
| Last updated | 2026-05-25 |

---

## Use cases on this page

Read-only summary from `GET /api/posts` and `GET /api/tags` (authenticated branch returns all post states). No single domain use-case doc; aggregates counts from post and tag lists.

---

## Screen states

| State | User sees |
|:---|:---|
| Loaded | Stat cards for total, published, and draft posts; tag count; link to create post |
| Error | Next.js error boundary (failed API fetch) |

---

## Shell

Inherits [admin shell](../shell.md).

---

## Tests

| Type | Location |
|:---|:---|
| Playwright | Not yet added for admin |
| Integration | Post and tag list queries covered by API integration tests |

Acceptance criteria: N/A (read-only dashboard aggregate; no dedicated use-case doc).
