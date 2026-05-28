# View Post by Slug

| Field | Value |
|:---|:---|
| Feature | `posts` |
| Status | Active (v1 complete) |
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

## UI projection

| App | Page doc | Role on page |
|:---|:---|:---|
| web | [post-detail.md](../../ui/web/pages/post-detail.md) | Article and comments |

Shell: [web shell.md](../../ui/web/shell.md)

### Operation states

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

| ID | Criterion | Test type |
|:---|:---|:---|
| AC-001 | Given a published post, when `GET /api/posts/{slug}` is called anonymously, then the post detail is returned. | BDD acceptance (`ViewPostBySlug.feature` @ac:AC-001) |
| AC-002 | Given a Draft post slug, when requested anonymously, then API returns 404. | BDD acceptance (`ViewPostBySlug.feature` @ac:AC-002) |
| AC-003 | Given a published post, when view-source is checked, then JSON-LD `BlogPosting` is present. | Playwright |

---

## Acceptance Coverage

| ID | Criterion summary | Risk | Required test type | BDD scenario | Plain API test | Domain/Application test | Manual only |
|:---|:---|:---|:---|:---|:---|:---|:---:|
| AC-001 | Published slug readable on API | Critical | BDD acceptance | Published post is readable by slug | | | |
| AC-002 | Draft slug hidden from public API | Critical | BDD acceptance | Draft post slug is not publicly readable | | | |
| AC-003 | JSON-LD on public page | High | Playwright | | | | |

**BDD decision:** BDD acceptance for AC-001 and AC-002 (public read contract). Playwright covers page metadata (AC-003).

---

## Out of Scope

Admin post view by ID. Comment storage in domain (Giscus is external).
