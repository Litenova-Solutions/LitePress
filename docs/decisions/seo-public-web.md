# SEO Strategy for Public Web

| Field | Value |
|:---|:---|
| Status | Accepted |
| Date | 2026-05-23 |

---

## Context

v1 requires full SEO compliance on `apps/web` for discoverability.

---

## Decision

Implement on all public routes:

1. **`generateMetadata`** on home, post detail, tags index, tag filter pages.
2. **Per-post metadata:** title, description (excerpt or derived), canonical URL, robots, Open Graph, Twitter cards.
3. **`app/sitemap.ts`:** dynamic entries for published posts and tag pages.
4. **`app/robots.ts`:** allow public paths; admin is a separate app (no admin paths in web robots).
5. **JSON-LD:** `BlogPosting` on post detail with `datePublished`, `author`, `headline`.
6. **Semantic HTML:** one `<h1>` per page, `<article>`, `<time dateTime>`.
7. **Slug URLs only** for posts and tags (no GUIDs in public URLs).
8. **Server Components** for all public read paths.
9. **Performance:** `priority` on cover images; sensible `loading` on list thumbnails.

Base URL from validated env `SITE_URL` (e.g. `https://blog.example.com`).

---

## Consequences

- Post detail page must await `params` and fetch before metadata generation.
- Sitemap regenerates on build/request based on API data.
- Production domain name required for canonical URLs (human input).

---

## References

- `docs/domain/posts/view-post-by-slug.md`
- `docs/domain/posts/list-published-posts.md`
- `docs/domain/tags/list-tags.md`
