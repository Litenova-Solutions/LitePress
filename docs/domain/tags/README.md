# Tags

| Field | Value |
|:---|:---|
| Status | Active |
| Last updated | 2026-05-23 |

---

## Ubiquitous Language

| Term | Definition | Maps To | Do Not Use |
|:---|:---|:---|:---|
| Tag | A keyword label associated with one or more Posts for categorization and filtering. | `Tag` aggregate | Category, Label, Topic |
| Tag Name | Human-readable label. Case-insensitive unique across all tags. Max 50 chars. | `TagName` | Label, Category |
| Tag Slug | URL-safe identifier derived from name. Immutable after creation. | `TagSlug` | Path, Handle |

---

## Aggregate: `Tag`

Identity: `TagId` (strongly typed `Guid`).

Stateless by existence: a tag is active while it exists in the database.

### Invariants

- Name required, 1–50 characters.
- Name must be unique (case-insensitive). Throws `TagNameAlreadyExistsException`.
- Slug derived from name at creation; updated on rename.
- Deleting a tag removes it from all posts that reference it (via command handler side effect).

---

## Domain Events

| Event | Raised when | Outbox required |
|:---|:---|:---:|
| `TagCreated` | `Tag` constructor | No (v1) |
| `TagRenamed` | `Tag.Rename()` | No |
| `TagDeleted` | `Tag.Delete()` | No |

---

## Persistence

| Table | Purpose |
|:---|:---|
| `tags` | Tag aggregate root |
| `post_tags` | Join table linking posts and tags |

---

## Use Cases

| Use case | Doc | Backend | Frontend |
|:---|:---|:---|:---|
| Create tag | [create-tag.md](create-tag.md) | `Tags/Create/` | `app/(dashboard)/tags/page.tsx` inline (admin) |
| Rename tag | [rename-tag.md](rename-tag.md) | `Tags/Rename/` | `features/tags/rename/` (admin) |
| Delete tag | [delete-tag.md](delete-tag.md) | `Tags/Delete/` | `features/tags/delete/` (admin) |
| List tags | [list-tags.md](list-tags.md) | `Tags/GetAllTags/` | `features/tags/list-tags/` (web); admin tags page inline |
| List posts by tag | [list-posts-by-tag.md](list-posts-by-tag.md) | `Posts/GetPostsByTag/` | `features/posts/list-published-posts/` (web; shared `PostList`) |
