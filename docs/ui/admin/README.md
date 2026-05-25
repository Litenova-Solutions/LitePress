# Admin — UI projection

App: `apps/admin` · Runbook: [apps/admin/README.md](../../../apps/admin/README.md)

Shell (dashboard chrome): [shell.md](shell.md)

---

## Pages

| Route | Page doc | Use cases composed | Domain modules |
|:---|:---|:---|:---|
| `/login` | [pages/login.md](pages/login.md) | (Auth.js; no domain use case) | `app/(auth)/login/` |
| `/` | [pages/dashboard.md](pages/dashboard.md) | (reads post list for stats) | `app/(dashboard)/page.tsx` |
| `/posts` | [pages/posts-list.md](pages/posts-list.md) | [list-published-posts](../../domain/posts/list-published-posts.md) (admin branch) | `app/(dashboard)/posts/page.tsx` |
| `/posts/new` | [pages/post-create.md](pages/post-create.md) | [create-post](../../domain/posts/create-post.md) | `domain/posts/create/` |
| `/posts/[id]` | [pages/post-editor.md](pages/post-editor.md) | [update-post](../../domain/posts/update-post.md), [publish-post](../../domain/posts/publish-post.md), [archive-post](../../domain/posts/archive-post.md), [delete-post](../../domain/posts/delete-post.md), [add-tag-to-post](../../domain/posts/add-tag-to-post.md) | `domain/posts/update/` |
| `/tags` | [pages/tags.md](pages/tags.md) | [list-tags](../../domain/tags/list-tags.md), [create-tag](../../domain/tags/create-tag.md), [rename-tag](../../domain/tags/rename-tag.md), [delete-tag](../../domain/tags/delete-tag.md) | `domain/tags/*`, dashboard page |

Auth and API proxy routes: see [admin-auth ADR](../../decisions/admin-auth.md).

---

## E2E

Admin Playwright suite: not yet added. Happy paths verified manually and via API integration tests.
