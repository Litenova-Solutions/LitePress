# View Post by Slug

| Field | Value |
|:---|:---|
| Feature | `posts` |
| Status | Active (backend complete; web UI partial, SEO not implemented) |
| Last updated | 2026-05-23 |

---

## Summary

Anonymous readers view a single published post at `/{slug}`. Content is rendered from ProseMirror JSON to safe HTML. Giscus comments appear below the article.

---

## Query

| Type | Name | Input | Output |
|:---|:---|:---|:---|
| Query | `GetPostBySlugQuery` | `Slug` | `PostDetailResult` |

### PostDetailResult fields

`PostId`, `Title`, `Slug`, `Content` (ProseMirror JSON), `Excerpt?`, `CoverImageUrl?`, `AuthorDisplayName`, `PostState`, `CreatedAt`, `PublishedAt?`, `Tags[]`

Returns 404 if slug not found or post not published.

---

## HTTP Endpoint

| Method | Path | Auth |
|:---|:---|:---|
| GET | `/api/posts/{slug}` | Anonymous |

Note: GUID paths route to `GetPostById` (admin). Slug paths route here.

---

## UI (web)

### Route and entry

- Route: `app/[slug]/page.tsx`
- Target domain entry: `domain/posts/view-by-slug/ViewPostBySlugPage.tsx`

### States

| State | Behavior |
|:---|:---|
| Loading | `loading.tsx` skeleton |
| Empty | `notFound()` for missing/unpublished slug |
| Error | `error.tsx` |
| Loaded | Article with semantic HTML, JSON-LD, Giscus widget |

### Rendering

- One `<h1>` for title.
- `<article>` wrapper with `<time dateTime={publishedAt}>`.
- ProseMirror JSON → sanitized HTML via shared renderer (not raw `dangerouslySetInnerHTML` on unvalidated input).
- Cover image with `priority` when present.

---

## SEO (mandatory for v1)

Per [docs/decisions/seo-public-web.md](../../decisions/seo-public-web.md):

- `generateMetadata`: title, description (excerpt or derived), canonical URL, robots, Open Graph, Twitter cards
- JSON-LD `BlogPosting` structured data
- Included in dynamic `app/sitemap.ts`

---

## Acceptance Criteria

1. Given a published post, when visiting `/{slug}`, then full content renders with correct metadata in view-source. (Playwright + manual)
2. Given a Draft post slug, when visited anonymously, then 404. (Integration)
3. Given a published post, when view-source is checked, then JSON-LD `BlogPosting` is present. (Playwright)

---

## Out of Scope

Admin post view by ID. Comment storage in domain (Giscus is external).
