# Ubiquitous Language Glossary

<!-- Last updated: 2026-05-21 -->

This file defines the key terms of the `Blog` bounded context so that all engineers
and agents use consistent language. Use these exact terms in all code, comments,
and documentation. When a term has a common synonym that MUST NOT be used in code,
it is listed in the "Do Not Use" column.

---

## Term Glossary

| Term | Definition | Maps To | Do Not Use |
|:---|:---|:---|:---|
| Post | A piece of content created by an Author. A Post moves through states: Draft → Published → Archived. The canonical unit of the blog. | `Post` aggregate | Article, Content, Entry, BlogPost |
| Author | A registered admin user who has been granted the ability to create, edit, publish, and archive Posts. Identity is always derived from the authenticated session, never from the request body. | `Author` aggregate | Writer, Creator, User |
| Tag | A short keyword label associated with one or more Posts for categorization and filtering. A Post may have up to 10 Tags. Tags are stand-alone entities managed separately from Posts. | `Tag` aggregate | Category, Label, Topic, Topic |
| Slug | A URL-safe string derived from a Post or Tag name, used to construct public-facing URLs. Slugs are immutable once a Post is published. | `PostSlug` value object, `TagSlug` value object | Path, URL, Handle |
| Draft | The initial and default state of a Post when first created. A Draft is not visible to the public. | `DraftPostState` | Unpublished, WIP, Private |
| Published | The state of a Post that is publicly visible on the blog. A Post transitions to Published via the Publish operation. | `PublishedPostState` | Live, Active, Visible, Public |
| Archived | The state of a Post that has been removed from public view without being permanently deleted. An Archived Post cannot be re-published without creating a new Post. | `ArchivedPostState` | Deleted, Hidden, Inactive, Unpublished |
| Rich Text Content | The body content of a Post, authored using the TipTap editor in the admin dashboard and stored as structured JSON (ProseMirror document). Rendered to HTML on the public frontend. | `PostContent` value object | Body, Description, Text, HTML |
| Excerpt | A short plain-text summary of a Post, either authored manually or auto-derived from the first paragraph. Used in listing views and social previews. | `PostExcerpt` value object | Summary, Description, Preview, Teaser |
| Cover Image | An optional image associated with a Post, stored as a URL reference to an uploaded file. Used in listing and detail views. | `PostCoverImageUrl` value object | Thumbnail, Hero, Banner, Featured Image |
| Comment | A public discussion thread on a Published Post, powered by Giscus (GitHub Discussions). Comments are not stored in the Blog domain — they are external to the bounded context. | _(external, not in domain model)_ | Reply, Response, Feedback |
| Admin Dashboard | The Next.js application (`apps/admin`) used by Authors to manage Posts and Tags. Protected by Auth.js v5 authentication. | `apps/admin/` | CMS, Backend, Admin Panel |
| Public Web | The Next.js application (`apps/web`) that serves Published Posts to anonymous readers. | `apps/web/` | Frontend, Blog Site, Public Site |

---

## Bounded Context Relationships

| External Concept | Local Alias | Notes |
|:---|:---|:---|
| Auth.js session user ID | `AuthorId` | The authenticated user's subject claim maps 1:1 to `AuthorId`. No separate identity domain exists in v1. |

---

## Related ADRs

- `standards/docs/adr/0002-clean-architecture-as-structural-foundation.md` — structural pattern.
- `standards/docs/adr/0003-cqrs-with-split-application-projects.md` — CQRS split.
