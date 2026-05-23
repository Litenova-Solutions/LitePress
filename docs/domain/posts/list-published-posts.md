# List Published Posts

| Field | Value |
|:---|:---|
| Feature | `posts` |
| Status | Active (backend complete; web UI partial) |
| Last updated | 2026-05-23 |

---

## Summary

Anonymous readers browse paginated published posts on the public home page. Authenticated admin callers receive all posts regardless of state via the same endpoint (see admin post list).

---

## Query

| Type | Name | Input | Output |
|:---|:---|:---|:---|
| Query | `GetPublishedPostsQuery` | `Pagination` (`PageNumber`, `PageSize`) | `PagedResult<PostSummaryResult>` |

Default page size: 10. Maximum: 50.

### PostSummaryResult fields

`PostId`, `Title`, `Slug`, `Excerpt?`, `CoverImageUrl?`, `AuthorDisplayName`, `PublishedAt?`, `Tags[]`

---

## HTTP Endpoint

| Method | Path | Auth |
|:---|:---|:---|
| GET | `/api/posts?page={n}&pageSize={n}` | Anonymous (published only) |

When `Authorization` header is present, returns all states via `GetAllPostsQuery` instead.

Optional `tag` query parameter routes to [list-posts-by-tag](../tags/list-posts-by-tag.md).

---

## UI (web)

### Route and entry

- Route: `app/page.tsx`
- Target domain entry: `domain/posts/list-published/ListPublishedPostsPage.tsx`

### States

| State | Behavior |
|:---|:---|
| Loading | `loading.tsx` skeleton |
| Empty | "No posts yet." message |
| Error | `error.tsx` with retry |
| Loaded | Paginated article list with tag badges and dates |

---

## SEO

Home page `generateMetadata` with site title and description. See [docs/decisions/seo-public-web.md](../../decisions/seo-public-web.md).

---

## Acceptance Criteria

1. Given published posts exist, when an anonymous user visits `/`, then only published posts appear, newest first. (Integration + Playwright)
2. Given page 2 is requested, when results exist, then pagination controls navigate correctly. (Playwright)
3. Given no published posts, when home loads, then empty state renders. (Playwright)

---

## Out of Scope

Admin post list (authenticated branch of same endpoint).
