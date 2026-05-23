# ProseMirror JSON Storage

| Field | Value |
|:---|:---|
| Status | Accepted |
| Date | 2026-05-23 |

---

## Context

Post body content is authored with TipTap in the admin dashboard and displayed on the public web.

---

## Decision

1. Store post content as **ProseMirror JSON** string in `PostContent` value object and `posts.content` column.
2. Admin TipTap editor serializes to/from ProseMirror JSON document format.
3. Public web renders JSON to **sanitized HTML** via a dedicated renderer. Never use `dangerouslySetInnerHTML` on unvalidated raw content.
4. Excerpt remains plain text (manual or derived from first text block).

---

## Consequences

- Public renderer must handle TipTap/ProseMirror node types used in admin schema.
- SEO description may derive from excerpt or first text block, not raw JSON.
- HTML storage in the database is forbidden.

---

## References

- `docs/domain/posts/README.md`
- `standards/docs/conventions/shared/security.md`
