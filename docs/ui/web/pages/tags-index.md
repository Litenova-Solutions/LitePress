# Tags index (`/tags`)

| Field | Value |
|:---|:---|
| App | `apps/web` |
| Route | `/tags` |
| Route shell | [app/tags/page.tsx](../../../apps/web/app/tags/page.tsx) |
| Domain entry | [TagsIndex.tsx](../../../apps/web/domain/tags/list-tags/TagsIndex.tsx) |

---

## Use cases on this page

| Use case | Doc |
|:---|:---|
| List tags | [list-tags.md](../../domain/tags/list-tags.md) |

---

## Visible states

| State | User sees |
|:---|:---|
| Loaded | All tags with post counts; each links to `/tags/[slug]` |
| Empty | Empty state message |

---

## Shell

Inherits [web shell](../shell.md).

---

## Tests

Acceptance criteria: [list-tags.md § Acceptance Criteria](../../domain/tags/list-tags.md#acceptance-criteria).
