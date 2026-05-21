# Frontend API Endpoints

<!-- Last updated: 2026-05-21 -->

This file documents which backend API endpoints the frontend actively consumes,
what they map to in the app folder structure, and project-specific notes about
authentication requirements and caching behavior.

Update this table in the same PR that adds or removes a fetch call in the frontend.

---

## Web App (`apps/web`) — Public Endpoints

| Endpoint | Feature | Used In | Auth Required | Cache Strategy | Notes |
|:---|:---|:---|:---|:---|:---|
| `GET /api/posts?page={n}&pageSize={n}` | Posts / List | `app/page.tsx` | No | `revalidateTag("posts", "hours")` | Returns paginated published posts. Default `pageSize=10`. |
| `GET /api/posts/{slug}` | Posts / Detail | `app/[slug]/page.tsx` | No | `revalidateTag("posts", "hours")` | Fetches by slug. Returns 404 if not found or not published; mapped to `notFound()`. |
| `GET /api/posts?tag={tagSlug}&page={n}` | Tags / Posts by tag | `app/tags/[tag]/page.tsx` | No | `revalidateTag("posts", "hours")` | Filters published posts by tag slug. Paginated. |
| `GET /api/tags` | Tags / List | `app/tags/page.tsx` | No | `revalidateTag("tags", "days")` | Returns all tags with post counts. |

---

## Admin App (`apps/admin`) — Authenticated Endpoints

| Endpoint | Feature | Used In | Auth Required | Cache Strategy | Notes |
|:---|:---|:---|:---|:---|:---|
| `GET /api/posts?page={n}&pageSize={n}` | Posts / List | `app/(dashboard)/posts/page.tsx` | Yes | No cache (always fresh) | Returns all posts regardless of state. Sorted by `CreatedAt` desc. |
| `GET /api/posts/{id}` | Posts / Edit | `app/(dashboard)/posts/[id]/page.tsx` | Yes | No cache | Fetches post by `PostId` for editing. |
| `POST /api/posts` | Posts / Create | `app/(dashboard)/posts/new/page.tsx` (Server Action) | Yes | Invalidates `revalidateTag("posts")` on success | Body: `{ title, content, excerpt?, coverImageUrl?, tagIds[] }`. |
| `PUT /api/posts/{id}` | Posts / Edit | `app/(dashboard)/posts/[id]/page.tsx` (Server Action) | Yes | Invalidates `revalidateTag("posts")` on success | Updates title, content, excerpt, cover image. Draft state only. |
| `POST /api/posts/{id}/publish` | Posts / Publish | `app/(dashboard)/posts/[id]/page.tsx` (Server Action) | Yes | Invalidates `revalidateTag("posts")` on success | Transitions post to Published state. |
| `POST /api/posts/{id}/archive` | Posts / Archive | `app/(dashboard)/posts/[id]/page.tsx` (Server Action) | Yes | Invalidates `revalidateTag("posts")` on success | Transitions post to Archived state. |
| `DELETE /api/posts/{id}` | Posts / Delete | `app/(dashboard)/posts/[id]/page.tsx` (Server Action) | Yes | Invalidates `revalidateTag("posts")` on success | Draft or Archived posts only. |
| `GET /api/tags` | Tags / List | `app/(dashboard)/tags/page.tsx` | Yes | No cache | Returns all tags with post counts. |
| `POST /api/tags` | Tags / Create | `app/(dashboard)/tags/page.tsx` (Server Action) | Yes | Invalidates `revalidateTag("tags")` on success | Body: `{ name }`. |
| `PUT /api/tags/{id}` | Tags / Rename | `app/(dashboard)/tags/page.tsx` (Server Action) | Yes | Invalidates `revalidateTag("tags", "posts")` on success | Body: `{ name }`. |
| `DELETE /api/tags/{id}` | Tags / Delete | `app/(dashboard)/tags/page.tsx` (Server Action) | Yes | Invalidates `revalidateTag("tags", "posts")` on success | Removes tag from all posts before deleting. |

---

## Authentication Notes

All admin endpoints require a valid Auth.js v5 session. The session token is stored
in an httpOnly cookie. The API client in `apps/admin/lib/api.ts` reads the session
and attaches it as a `Bearer` header automatically.

Unauthenticated requests to auth-required endpoints return HTTP 401. The admin app
uses Auth.js `middleware` (via `auth.ts`) to redirect unauthenticated users to
`/(auth)/login` before they reach the dashboard routes.

The public web app (`apps/web`) does not use authentication. All public endpoints
are unauthenticated and return only `PublishedPostState` content.

---

## Maintenance Notes

- The API base URL is configured via environment variable `NEXT_PUBLIC_API_URL` in both apps.
- Server Actions call the API directly from the server runtime using the session cookie.
- `revalidateTag` calls must include a cache life argument per the standards convention.
- Run `pnpm generate:api` (when configured) to regenerate TypeScript types from the
  OpenAPI spec whenever the backend API changes.
