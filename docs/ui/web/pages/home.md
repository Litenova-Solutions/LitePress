# Home (`/`)

| Field | Value |
|:---|:---|
| App | `apps/web` |
| Route | `/` |
| Route shell | [app/page.tsx](../../../apps/web/app/page.tsx) |
| Domain entry | [PostList.tsx](../../../apps/web/domain/posts/list-published-posts/PostList.tsx) |

---

## Use cases on this page

| Use case | Doc |
|:---|:---|
| List published posts | [list-published-posts.md](../../domain/posts/list-published-posts.md) |

Optional `?tag=` filter delegates to [list-posts-by-tag.md](../../domain/tags/list-posts-by-tag.md) query branch; list UI is shared with [posts-by-tag](posts-by-tag.md).

---

## Visible states

| State | User sees |
|:---|:---|
| Loaded (posts) | Paginated cards, newest first; title links to `/[slug]` |
| Empty | "No posts yet." |
| Error | Next.js `error.tsx` with retry |

Loading skeleton: optional via `loading.tsx`.

---

## Shell

Inherits [web shell](../shell.md) (header, footer, content column).

---

## Tests

| Type | Location |
|:---|:---|
| Playwright | [e2e/home.spec.ts](../../../apps/web/e2e/home.spec.ts), [e2e/publish-flow.spec.ts](../../../apps/web/e2e/publish-flow.spec.ts) |
| Layout | [e2e/layout.spec.ts](../../../apps/web/e2e/layout.spec.ts) |

Acceptance criteria: [list-published-posts.md § Acceptance Criteria](../../domain/posts/list-published-posts.md#acceptance-criteria).
