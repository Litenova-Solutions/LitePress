# List Posts by Tag

| Field | Value |
|:---|:---|
| Feature | `tags` |
| Status | Active (v1 complete) |
| Last updated | 2026-05-23 |

---

## Summary

Anonymous readers browse published posts filtered by tag slug at `/tags/{slug}`.

---

## Query

| Type | Name | Input | Output |
|:---|:---|:---|:---|
| Query | `GetPostsByTagQuery` | `TagSlug`, `Pagination` | `PagedResult<PostSummaryResult>` |

Also available via `GET /api/posts?tag={slug}&page={n}`.

---

## HTTP Endpoint

| Method | Path | Auth |
|:---|:---|:---|
| GET | `/api/posts?tag={slug}&page={n}&pageSize={n}` | Anonymous |

Returns empty items if tag slug not found (not 404).

---

## UI (web)

### Route and entry

- Route: `app/tags/[slug]/page.tsx`
- Target: `domain/tags/list-posts/ListPostsByTagPage.tsx`

### States

| State | Behavior |
|:---|:---|
| Loading | Skeleton list |
| Empty | "No posts with this tag." |
| Error | Error boundary |
| Loaded | Paginated post list with tag name as `<h1>` |

---

## SEO

Per-tag page metadata: title "Posts tagged: {name}", canonical `/tags/{slug}`. Included in sitemap.

---

## Acceptance Criteria

1. Given published posts with tag "dotnet", when visiting `/tags/dotnet`, then only matching posts appear. (Playwright)
2. Given unknown tag slug, when visited, then empty list (not 500). (Integration)

---

## Out of Scope

Tag autocomplete search.
