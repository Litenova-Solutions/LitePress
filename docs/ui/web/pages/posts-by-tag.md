# Posts by tag (`/tags/[slug]`)

| Field | Value |
|:---|:---|
| App | `apps/web` |
| Route | `/tags/[slug]` |
| Route shell | [app/tags/[slug]/page.tsx](../../../apps/web/app/tags/[slug]/page.tsx) |
| Domain entry | [PostList.tsx](../../../apps/web/domain/posts/list-published-posts/PostList.tsx) (reused) |

---

## Use cases on this page

| Use case | Doc |
|:---|:---|
| List posts by tag | [list-posts-by-tag.md](../../domain/tags/list-posts-by-tag.md) |
| List published posts (shared list UI) | [list-published-posts.md](../../domain/posts/list-published-posts.md) |

---

## Visible states

| State | User sees |
|:---|:---|
| Loaded | Filtered post list for the tag slug |
| Empty | No posts for this tag |
| Unknown tag | 404 |

---

## Shell

Inherits [web shell](../shell.md).

---

## Tests

Acceptance criteria: [list-posts-by-tag.md § Acceptance Criteria](../../domain/tags/list-posts-by-tag.md#acceptance-criteria).
