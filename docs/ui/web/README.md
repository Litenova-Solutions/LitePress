# Public web — UI projection

App: `apps/web` · Runbook: [apps/web/README.md](../../../apps/web/README.md)

Shell (layout shared by all routes): [shell.md](shell.md)

---

## Pages

| Route | Page doc | Primary use cases | Feature modules |
|:---|:---|:---|:---|
| `/` | [pages/home.md](pages/home.md) | [list-published-posts](../../domain/posts/list-published-posts.md), [list-posts-by-tag](../../domain/tags/list-posts-by-tag.md) (`?tag=` branch) | `features/posts/list-published-posts/` |
| `/[slug]` | [pages/post-detail.md](pages/post-detail.md) | [view-post-by-slug](../../domain/posts/view-post-by-slug.md) | `features/posts/view-post-by-slug/` |
| `/tags` | [pages/tags-index.md](pages/tags-index.md) | [list-tags](../../domain/tags/list-tags.md) | `features/tags/list-tags/` |
| `/tags/[slug]` | [pages/posts-by-tag.md](pages/posts-by-tag.md) | [list-posts-by-tag](../../domain/tags/list-posts-by-tag.md), [list-published-posts](../../domain/posts/list-published-posts.md) (shared list UI) | `features/posts/list-published-posts/` |

Non-page routes (`/sitemap.xml`, `/robots.txt`) are defined in [seo-public-web ADR](../../decisions/seo-public-web.md). No page doc.

---

## E2E tests

| Spec | Covers |
|:---|:---|
| [e2e/home.spec.ts](../../../apps/web/e2e/home.spec.ts) | Home loads |
| [e2e/layout.spec.ts](../../../apps/web/e2e/layout.spec.ts) | Shell footer placement |
| [e2e/publish-flow.spec.ts](../../../apps/web/e2e/publish-flow.spec.ts) | Published post on home and slug page |

---

## Approved page doc example

Multi-use-case composition with shared list UI: [pages/home.md](pages/home.md) and [pages/posts-by-tag.md](pages/posts-by-tag.md).
