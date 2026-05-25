# Post editor (`/posts/[id]`)

| Field | Value |
|:---|:---|
| App | `apps/admin` |
| Route | `/posts/[id]` |
| Route shell | [app/(dashboard)/posts/[id]/page.tsx](../../../apps/admin/app/(dashboard)/posts/[id]/page.tsx) |
| Feature entry | [EditPostForm.tsx](../../../apps/admin/features/posts/update/EditPostForm.tsx) |
| Last updated | 2026-05-25 |

One page composes several domain operations. Domain rules stay in use-case and feature docs; this page doc maps which actions appear when.

---

## Use cases on this page

| Use case | Doc | Visible when |
|:---|:---|:---|
| Update post | [update-post.md](../../domain/posts/update-post.md) | Draft — form editable |
| Publish post | [publish-post.md](../../domain/posts/publish-post.md) | Draft — Publish action |
| Archive post | [archive-post.md](../../domain/posts/archive-post.md) | Published — Archive action |
| Delete post | [delete-post.md](../../domain/posts/delete-post.md) | Draft or Archived — Delete (not when Published; see [delete-post.md](../../domain/posts/delete-post.md)) |
| Add / remove tag | [add-tag-to-post.md](../../domain/posts/add-tag-to-post.md) | Draft — tag toggle list |

---

## Screen states

| State | User sees |
|:---|:---|
| Loading | Loading placeholder while fetching post |
| Error | Toast with API problem detail |

---

## Content modes

| Mode | User sees |
|:---|:---|
| Draft | Editable form, Publish, Delete, tag assignment |
| Published | Read-only content, Archive |
| Archived | Read-only, Delete |

Published posts are read-only per [update-post.md](../../domain/posts/update-post.md). Delete when Published must fail with 409 per [delete-post.md](../../domain/posts/delete-post.md).

---

## Shell

Inherits [admin shell](../shell.md).

---

## Tests

| Type | Location |
|:---|:---|
| Playwright | Not yet added for admin |
| Integration | Per linked use-case API tests |

Acceptance criteria: see each linked use-case doc § Acceptance Criteria.
