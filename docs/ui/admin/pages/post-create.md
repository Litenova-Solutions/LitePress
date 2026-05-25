# Create post (`/posts/new`)

| Field | Value |
|:---|:---|
| App | `apps/admin` |
| Route | `/posts/new` |
| Route shell | [app/(dashboard)/posts/new/page.tsx](../../../apps/admin/app/(dashboard)/posts/new/page.tsx) |
| Feature entry | [CreatePostForm.tsx](../../../apps/admin/features/posts/create/CreatePostForm.tsx) |
| Last updated | 2026-05-25 |

---

## Use cases on this page

| Use case | Doc |
|:---|:---|
| Create post | [create-post.md](../../domain/posts/create-post.md) |

---

## Screen states

| State | User sees |
|:---|:---|
| Editing | TipTap editor, title, excerpt, cover URL fields |
| Submitting | Disabled submit, loading label |
| Success | Redirect to `/posts/[id]` |
| Error | Inline alert and toast |

---

## Shell

Inherits [admin shell](../shell.md).

---

## Tests

| Type | Location |
|:---|:---|
| Playwright | Not yet added for admin |
| Integration | [create-post.md](../../domain/posts/create-post.md) API tests |

Acceptance criteria: [create-post.md § Acceptance Criteria](../../domain/posts/create-post.md#acceptance-criteria).
