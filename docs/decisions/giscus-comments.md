# Giscus Comments

| Field | Value |
|:---|:---|
| Status | Accepted |
| Date | 2026-05-23 |

---

## Context

Published posts should support reader comments without storing comment data in the blog database.

---

## Decision

Use [Giscus](https://giscus.app/) (GitHub Discussions-backed) on the public post detail page only.

- Comments are external to the `LitePress` bounded context.
- `GiscusComments.tsx` is a client component (embed script requires browser).
- Configuration via env: `NEXT_PUBLIC_GISCUS_REPO`, `NEXT_PUBLIC_GISCUS_REPO_ID`, `NEXT_PUBLIC_GISCUS_CATEGORY_ID` (public values only).

---

## Consequences

- No comment moderation in admin v1.
- Comments require GitHub account to post (Giscus default).
- Not included in sitemap or JSON-LD.

---

## References

- `docs/domain/posts/view-post-by-slug.md`
