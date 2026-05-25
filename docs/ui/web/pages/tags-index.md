# Tags index (`/tags`)

| Field | Value |
|:---|:---|
| App | `apps/web` |
| Route | `/tags` |
| Route shell | [app/tags/page.tsx](../../../apps/web/app/tags/page.tsx) |
| Feature entry | [TagsIndex.tsx](../../../apps/web/features/tags/list-tags/TagsIndex.tsx) |
| Last updated | 2026-05-25 |

---

## Use cases on this page

| Use case | Doc |
|:---|:---|
| List tags | [list-tags.md](../../domain/tags/list-tags.md) |

---

## Screen states

| State | User sees |
|:---|:---|
| Loaded | All tags with post counts; each links to `/tags/[slug]` |
| Empty | Empty state message |
| Error | Next.js error boundary (failed API fetch) |

---

## Shell

Inherits [web shell](../shell.md).

---

## Tests

| Type | Location |
|:---|:---|
| Playwright | (covered by tag navigation in publish flow where applicable) |

Acceptance criteria: [list-tags.md § Acceptance Criteria](../../domain/tags/list-tags.md#acceptance-criteria).
