# Posts by tag (`/tags/[slug]`)

| Field | Value |
|:---|:---|
| App | `apps/web` |
| Route | `/tags/[slug]` |
| Route shell | [app/tags/[slug]/page.tsx](../../../apps/web/app/tags/[slug]/page.tsx) |
| Feature entry | [PostList.tsx](../../../apps/web/features/posts/list-published-posts/PostList.tsx) (reused) |
| Last updated | 2026-05-25 |

---

## Use cases on this page

| Use case | Doc | Notes |
|:---|:---|:---|
| List posts by tag | [list-posts-by-tag.md](../../domain/tags/list-posts-by-tag.md) | Primary filter for this route |
| List published posts | [list-published-posts.md](../../domain/posts/list-published-posts.md) | Shared list UI component |

The same `PostList` component and query branch also power the optional `?tag=` filter on [home](home.md).

---

## Screen states

| State | User sees |
|:---|:---|
| Loaded | Filtered post list for the tag slug |
| Empty | No posts for this tag |
| Unknown tag | Empty list (API returns empty items, not 404) |
| Error | Next.js error boundary |

---

## Shell

Inherits [web shell](../shell.md).

---

## Tests

| Type | Location |
|:---|:---|
| Playwright | Tag filter paths in integration/e2e where covered |

Acceptance criteria: [list-posts-by-tag.md § Acceptance Criteria](../../domain/tags/list-posts-by-tag.md#acceptance-criteria).
