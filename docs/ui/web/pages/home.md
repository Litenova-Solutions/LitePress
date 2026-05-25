# Home (`/`)

| Field | Value |
|:---|:---|
| App | `apps/web` |
| Route | `/` |
| Route shell | [app/page.tsx](../../../apps/web/app/page.tsx) |
| Feature entry | [PostList.tsx](../../../apps/web/features/posts/list-published-posts/PostList.tsx) |
| Last updated | 2026-05-25 |

---

## Use cases on this page

| Use case | Doc | Notes |
|:---|:---|:---|
| List published posts | [list-published-posts.md](../../domain/posts/list-published-posts.md) | Primary list |
| List posts by tag | [list-posts-by-tag.md](../../domain/tags/list-posts-by-tag.md) | Optional `?tag=` query branch; shared list UI with [posts-by-tag](posts-by-tag.md) |

---

## Screen states

| State | User sees |
|:---|:---|
| Loading | Optional skeleton via `loading.tsx` |
| Loaded | Paginated cards, newest first; title links to `/[slug]` |
| Empty | "No posts yet." |
| Error | Next.js `error.tsx` with retry |

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
