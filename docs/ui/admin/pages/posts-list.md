# Posts list (`/posts`)

| Field | Value |
|:---|:---|
| App | `apps/admin` |
| Route | `/posts` |
| Route shell | [app/(dashboard)/posts/page.tsx](../../../apps/admin/app/(dashboard)/posts/page.tsx) |

---

## Use cases on this page

| Use case | Doc |
|:---|:---|
| List posts (all states, authenticated) | [list-published-posts.md](../../domain/posts/list-published-posts.md) (admin branch of same endpoint) |

---

## Visible states

| State | User sees |
|:---|:---|
| Loaded | Table of posts with status badge, link to editor |
| Empty | Empty table |

---

## Shell

[Dashboard shell](../shell.md).

---

## Tests

Acceptance criteria for public list differ; admin list is authenticated branch of the same query. See [list-published-posts.md § Out of Scope](../../domain/posts/list-published-posts.md#out-of-scope).
