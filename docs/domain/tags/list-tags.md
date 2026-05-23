# List Tags

| Field | Value |
|:---|:---|
| Feature | `tags` |
| Status | Active (v1 complete) |
| Last updated | 2026-05-23 |

---

## Summary

Readers and Authors view all tags with post counts. Public endpoint requires no authentication.

---

## Query

| Type | Name | Input | Output |
|:---|:---|:---|:---|
| Query | `GetAllTagsQuery` | (none) | `IReadOnlyList<TagResult>` |

### TagResult fields

`TagId`, `Name`, `Slug`, `PostCount`

Post count reflects published posts only on the public side.

---

## HTTP Endpoint

| Method | Path | Auth |
|:---|:---|:---|
| GET | `/api/tags` | Anonymous |

---

## UI

### Web

- Route: `app/tags/page.tsx` (to be created)
- Target: `domain/tags/list/ListTagsPage.tsx`
- Lists all tags linking to `/tags/{slug}`

### Admin

- Route: `app/(dashboard)/tags/page.tsx`
- Target: `domain/tags/list/AdminTagsPage.tsx`
- Includes create, rename, delete actions

---

## SEO

Tags index page needs `generateMetadata` and sitemap entry. See [docs/decisions/seo-public-web.md](../../decisions/seo-public-web.md).

---

## Acceptance Criteria

1. Given tags exist, when visiting `/tags`, then all tags render with post counts. (Playwright)
2. Given no tags, when visiting `/tags`, then empty state renders. (Playwright)

---

## Out of Scope

Pagination (tag count is bounded by design).
