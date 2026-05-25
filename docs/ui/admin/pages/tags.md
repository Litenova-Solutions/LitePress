# Tags (`/tags`)

| Field | Value |
|:---|:---|
| App | `apps/admin` |
| Route | `/tags` |
| Route shell | [app/(dashboard)/tags/page.tsx](../../../apps/admin/app/(dashboard)/tags/page.tsx) |

---

## Use cases on this page

| Use case | Doc |
|:---|:---|
| List tags | [list-tags.md](../../domain/tags/list-tags.md) |
| Create tag | [create-tag.md](../../domain/tags/create-tag.md) |
| Rename tag | [rename-tag.md](../../domain/tags/rename-tag.md) |
| Delete tag | [delete-tag.md](../../domain/tags/delete-tag.md) |

Rename and delete actions live in `domain/tags/rename/` and `domain/tags/delete/` components on this page.

---

## Visible states

| State | User sees |
|:---|:---|
| Loaded | Tag table with create form and row actions |
| Empty | Empty list with create form |

---

## Shell

[Dashboard shell](../shell.md).

---

## Tests

Acceptance criteria in each linked use-case doc.
