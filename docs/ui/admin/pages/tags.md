# Tags (`/tags`)

| Field | Value |
|:---|:---|
| App | `apps/admin` |
| Route | `/tags` |
| Route shell | [app/(dashboard)/tags/page.tsx](../../../apps/admin/app/(dashboard)/tags/page.tsx) |
| Feature entry | (inline in route shell); [RenameTagButton.tsx](../../../apps/admin/features/tags/rename/RenameTagButton.tsx), [DeleteTagButton.tsx](../../../apps/admin/features/tags/delete/DeleteTagButton.tsx) |
| Last updated | 2026-05-25 |

---

## Use cases on this page

| Use case | Doc |
|:---|:---|
| List tags | [list-tags.md](../../domain/tags/list-tags.md) |
| Create tag | [create-tag.md](../../domain/tags/create-tag.md) |
| Rename tag | [rename-tag.md](../../domain/tags/rename-tag.md) |
| Delete tag | [delete-tag.md](../../domain/tags/delete-tag.md) |

Create form and tag table live in the route shell. Rename and delete row actions live in `features/tags/rename/` and `features/tags/delete/`.

---

## Screen states

| State | User sees |
|:---|:---|
| Loaded | Tag table with create form and row actions |
| Empty | Empty list with create form |
| Error | Next.js error boundary (failed API fetch) |

---

## Shell

Inherits [admin shell](../shell.md).

---

## Tests

| Type | Location |
|:---|:---|
| Playwright | Not yet added for admin |
| Integration | Tag CRUD covered by API integration tests |

Acceptance criteria: see each linked use-case doc § Acceptance Criteria.
