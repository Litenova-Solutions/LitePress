# LitePress — Domain Map

| Field | Value |
|:---|:---|
| Bounded context | LitePress |
| Namespaces | `LiteNova.Blog.*` |
| Last updated | 2026-05-23 |

> **Plain-language overview:** [How LitePress works](../how-it-works.md) · **Technical docs:** [technical/README.md](../technical/README.md)

---

## Features

| Feature | Aggregate(s) | Feature README |
|:---|:---|:---|
| Posts | `Post` | [posts/README.md](posts/README.md) |
| Tags | `Tag` | [tags/README.md](tags/README.md) |
| Authors | `Author` | [authors/README.md](authors/README.md) |

---

## Use Cases

### Posts

| Use case | Doc | Backend | Web (`apps/web`) | Admin (`apps/admin`) |
|:---|:---|:---|:---|:---|
| Create post | [create-post.md](posts/create-post.md) | Implemented | — | Implemented |
| Update post | [update-post.md](posts/update-post.md) | Implemented | — | Implemented |
| Publish post | [publish-post.md](posts/publish-post.md) | Implemented | — | Implemented |
| Archive post | [archive-post.md](posts/archive-post.md) | Implemented | — | Implemented |
| Delete post | [delete-post.md](posts/delete-post.md) | Implemented | — | Implemented |
| Add tag to post | [add-tag-to-post.md](posts/add-tag-to-post.md) | Implemented | — | Implemented |
| List published posts | [list-published-posts.md](posts/list-published-posts.md) | Implemented | Implemented | — |
| View post by slug | [view-post-by-slug.md](posts/view-post-by-slug.md) | Implemented | Implemented | — |

### Tags

| Use case | Doc | Backend | Web | Admin |
|:---|:---|:---|:---|:---|
| Create tag | [create-tag.md](tags/create-tag.md) | Implemented | — | Implemented |
| Rename tag | [rename-tag.md](tags/rename-tag.md) | Implemented | — | Implemented |
| Delete tag | [delete-tag.md](tags/delete-tag.md) | Implemented | — | Implemented |
| List tags | [list-tags.md](tags/list-tags.md) | Implemented | Implemented (`/tags`) | Implemented |
| List posts by tag | [list-posts-by-tag.md](tags/list-posts-by-tag.md) | Implemented | Implemented (`/tags/[slug]`) | — |

### Authors

| Use case | Doc | Backend | Web | Admin |
|:---|:---|:---|:---|:---|
| Register author | [register-author.md](authors/register-author.md) | Implemented (middleware) | — | Automatic on login |

---

## Cross-Domain Notes

- **Dual frontends:** `apps/web` (public, SEO-first) and `apps/admin` (authenticated authoring). Each app owns its own `domain/{feature}/{use-case}/` tree. No cross-imports between apps.
- **Comments:** Giscus (GitHub Discussions) on published posts. External to the domain; see [docs/decisions/giscus-comments.md](../decisions/giscus-comments.md).
- **Scheduled publishing:** Out of v1 scope; see [docs/decisions/v1-scope-deferrals.md](../decisions/v1-scope-deferrals.md).
- **Author identity:** `AuthorId` is derived from the JWT `sub` claim. Never accepted from request bodies.

---

## Project Decisions

Blog-specific ADRs live under [docs/decisions/](../decisions/README.md).
