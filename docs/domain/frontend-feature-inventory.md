# Frontend Feature Inventory

<!-- Last updated: 2026-05-21 -->

This file gives agents a complete map of all frontend features, use cases, and routes.
Consult it before creating a new page or feature component to prevent duplicate
routes, naming conflicts, and misaligned use case boundaries.

This file mirrors the backend feature inventory at `docs/domain/feature-inventory.md`.
Every frontend use case corresponds to a backend use case.

Update this table in the same PR that adds or removes a page or feature.

---

## Web App (`apps/web`) — Public

| Feature | Use Case | Route | Page Component | Type | Status |
|:---|:---|:---|:---|:---|:---|
| Posts | List latest published posts | `/` | `app/page.tsx` | Server | Planned |
| Posts | View post detail | `/[slug]` | `app/[slug]/page.tsx` | Server | Planned |
| Tags | List all tags | `/tags` | `app/tags/page.tsx` | Server | Planned |
| Tags | List posts by tag | `/tags/[tag]` | `app/tags/[tag]/page.tsx` | Server | Planned |

**Type column values:**
- **Server**: Page component is a React Server Component. Data is fetched on the server.
- **Client**: Page component is a client component or delegates to a primarily client-side feature component.

---

## Admin App (`apps/admin`) — Authenticated

| Feature | Use Case | Route | Page Component | Type | Status |
|:---|:---|:---|:---|:---|:---|
| Auth | Login | `/(auth)/login` | `app/(auth)/login/page.tsx` | Server | Planned |
| Dashboard | Overview | `/(dashboard)` | `app/(dashboard)/page.tsx` | Server | Planned |
| Posts | List all posts (all states) | `/(dashboard)/posts` | `app/(dashboard)/posts/page.tsx` | Server | Planned |
| Posts | Create new post | `/(dashboard)/posts/new` | `app/(dashboard)/posts/new/page.tsx` | Client | Planned |
| Posts | Edit existing post | `/(dashboard)/posts/[id]` | `app/(dashboard)/posts/[id]/page.tsx` | Client | Planned |
| Tags | List all tags | `/(dashboard)/tags` | `app/(dashboard)/tags/page.tsx` | Server | Planned |

---

## Shared Components

Components used by more than one feature are listed here.

### Web (`apps/web`)

| Component | Used By | Notes |
|:---|:---|:---|
| `components/Header.tsx` | All public pages | Site navigation with tag links. |
| `components/Footer.tsx` | All public pages | Site footer. |
| `components/PostCard.tsx` | Posts: list, tag filter | Summary card for a Post in a list. |
| `components/PostList.tsx` | Posts: list, tag filter | Renders a list of `PostCard` components. |
| `components/TagBadge.tsx` | Posts: list, detail | Inline tag label with link to tag page. |
| `components/GiscusComments.tsx` | Posts: detail | Giscus comment widget (client component). |

### Admin (`apps/admin`)

| Component | Used By | Notes |
|:---|:---|:---|
| `components/Sidebar.tsx` | All dashboard pages | Navigation sidebar. |
| `components/PostForm.tsx` | Posts: create, edit | Form with TipTap editor for Post authoring. Client component. |
| `components/TipTapEditor.tsx` | Posts: create, edit | Rich text editor. Must be `'use client'`. |
| `components/PostStatusBadge.tsx` | Posts: list, edit | Displays Draft/Published/Archived state. |

---

## Maintenance Notes

- The `/tags` route on the web app is not in the current directory structure. Add it when implementing tag browsing.
- `PostForm.tsx` is a client component because TipTap requires browser APIs.
- The Giscus comment widget is a client-only component because it uses the GitHub Discussions embed script.
- All admin routes under `/(dashboard)` require an authenticated session. The Auth.js middleware enforces this.
