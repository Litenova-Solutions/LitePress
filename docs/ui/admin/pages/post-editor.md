# Post editor (`/posts/[id]`)

| Field | Value |
|:---|:---|
| App | `apps/admin` |
| Route | `/posts/[id]` |
| Route shell | [app/(dashboard)/posts/[id]/page.tsx](../../../apps/admin/app/(dashboard)/posts/[id]/page.tsx) |
| Domain entry | [EditPostForm.tsx](../../../apps/admin/domain/posts/update/EditPostForm.tsx) |

One page composes several domain operations. Domain rules stay in use-case and feature docs; this page doc maps which actions appear when.

---

## Use cases on this page

| Use case | Doc | Visible when |
|:---|:---|:---|
| Update post | [update-post.md](../../domain/posts/update-post.md) | `postState === Draft` — form editable |
| Publish post | [publish-post.md](../../domain/posts/publish-post.md) | Draft — Publish action |
| Archive post | [archive-post.md](../../domain/posts/archive-post.md) | Published — Archive action |
| Delete post | [delete-post.md](../../domain/posts/delete-post.md) | Draft or Archived — Delete action (domain invariant: **not** when Published) |
| Add / remove tag | [add-tag-to-post.md](../../domain/posts/add-tag-to-post.md) | Tag toggle list |

Published posts: form read-only per [update-post.md](../../domain/posts/update-post.md) domain rule.

Delete when Published must fail with 409 per [delete-post.md](../../domain/posts/delete-post.md) and invariant in [posts/README.md](../../domain/posts/README.md).

---

## Visible states

| State | User sees |
|:---|:---|
| Loading | Loading placeholder |
| Draft | Editable form, Publish, Delete, tag assignment |
| Published | Read-only content, Archive |
| Archived | Read-only, Delete |
| Error | Toast with API problem detail |

---

## Shell

[Dashboard shell](../shell.md).

---

## Tests

Per use-case acceptance criteria in linked docs. No dedicated Playwright spec yet.
