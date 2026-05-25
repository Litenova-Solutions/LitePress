# Create post (`/posts/new`)

| Field | Value |
|:---|:---|
| App | `apps/admin` |
| Route | `/posts/new` |
| Route shell | [app/(dashboard)/posts/new/page.tsx](../../../apps/admin/app/(dashboard)/posts/new/page.tsx) |
| Domain entry | [CreatePostForm.tsx](../../../apps/admin/domain/posts/create/CreatePostForm.tsx) |

---

## Use cases on this page

| Use case | Doc |
|:---|:---|
| Create post | [create-post.md](../../domain/posts/create-post.md) |

---

## Visible states

| State | User sees |
|:---|:---|
| Editing | TipTap editor, title, excerpt, cover URL fields |
| Submitting | Disabled submit, loading label |
| Success | Redirect to `/posts/[id]` |
| Error | Inline alert and toast |

---

## Shell

[Dashboard shell](../shell.md).

---

## Tests

Acceptance criteria: [create-post.md § Acceptance Criteria](../../domain/posts/create-post.md#acceptance-criteria).
