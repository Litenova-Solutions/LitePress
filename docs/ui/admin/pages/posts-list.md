# Posts list (`/posts`)

| Field | Value |
|:---|:---|
| App | `apps/admin` |
| Route | `/posts` |
| Route shell | [app/(dashboard)/posts/page.tsx](../../../apps/admin/app/(dashboard)/posts/page.tsx) |
| Feature entry | (inline in route shell) |
| Last updated | 2026-05-25 |

---

## Use cases on this page

| Use case | Doc |
|:---|:---|
| List posts (all states, authenticated) | [list-published-posts.md](../../domain/posts/list-published-posts.md) (admin branch of same endpoint) |

---

## Screen states

| State | User sees |
|:---|:---|
| Loaded | Table of posts with status badge, link to editor |
| Empty | Empty table message |
| Error | Next.js error boundary (failed API fetch) |

---

## Shell

Inherits [admin shell](../shell.md).

---

## Tests

| Type | Location |
|:---|:---|
| Playwright | Not yet added for admin |
| Integration | Authenticated list branch in API integration tests |

Acceptance criteria for the public list differ; admin list is the authenticated branch of the same query. See [list-published-posts.md § Out of Scope](../../domain/posts/list-published-posts.md#out-of-scope).
