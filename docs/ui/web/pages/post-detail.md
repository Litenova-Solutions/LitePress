# Post detail (`/[slug]`)

| Field | Value |
|:---|:---|
| App | `apps/web` |
| Route | `/[slug]` |
| Route shell | [app/[slug]/page.tsx](../../../apps/web/app/[slug]/page.tsx) |
| Domain entry | [PostArticle.tsx](../../../apps/web/domain/posts/view-post-by-slug/PostArticle.tsx), [GiscusComments.tsx](../../../apps/web/domain/posts/view-post-by-slug/GiscusComments.tsx) |

---

## Use cases on this page

| Use case | Doc |
|:---|:---|
| View post by slug | [view-post-by-slug.md](../../domain/posts/view-post-by-slug.md) |

---

## Visible states

| State | User sees |
|:---|:---|
| Loaded | Title, author, date, cover image, prose body, tags, Giscus comments |
| Not found / unpublished | Next.js `notFound()` (404) |

---

## Shell

Inherits [web shell](../shell.md).

SEO and JSON-LD: [seo-public-web ADR](../../decisions/seo-public-web.md), [view-post-by-slug.md § SEO](../../domain/posts/view-post-by-slug.md#seo-mandatory-for-v1).

---

## Tests

| Type | Location |
|:---|:---|
| Playwright | [e2e/publish-flow.spec.ts](../../../apps/web/e2e/publish-flow.spec.ts) |

Acceptance criteria: [view-post-by-slug.md § Acceptance Criteria](../../domain/posts/view-post-by-slug.md#acceptance-criteria).
