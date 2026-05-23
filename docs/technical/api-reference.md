# API Reference

REST API served by `LiteNova.LitePress.WebApi`. OpenAPI spec: **`GET /openapi/v1.json`** when the API is running.

TypeScript types: `@litenova/api-types` (generated from OpenAPI).

---

## Base URL

| Environment | URL |
|:---|:---|
| Local (standalone) | `http://localhost:5000` |
| Aspire | Check Aspire dashboard for assigned port |

All paths below are relative to the base URL.

---

## Authentication

| Route type | Auth |
|:---|:---|
| Public reads (published posts, tags) | None |
| Mutating routes | `Authorization: Bearer <JWT>` |

JWT is HS256. The `sub` claim identifies the author (GitHub user ID from admin). `EnsureAuthorMiddleware` registers the author on first authenticated request.

**Never** send `AuthorId` in request bodies.

---

## Posts

| Method | Path | Auth | Description |
|:---|:---|:---|:---|
| `GET` | `/api/posts` | Public | List posts. Query: `page`, `pageSize`, `tag`. Public callers receive **published** posts only; authenticated callers receive all states. |
| `GET` | `/api/posts/{slug}` | Public | Published post by slug |
| `GET` | `/api/posts/{id}` | Bearer | Post by ID (admin) |
| `POST` | `/api/posts` | Bearer | Create draft. Body: `title`, `content`, `excerpt?`, `coverImageUrl?`, `tagIds?` |
| `PUT` | `/api/posts/{id}` | Bearer | Update draft. Body: `title`, `content`, `excerpt?`, `coverImageUrl?` |
| `POST` | `/api/posts/{id}/publish` | Bearer | Publish draft |
| `POST` | `/api/posts/{id}/archive` | Bearer | Archive published post |
| `DELETE` | `/api/posts/{id}` | Bearer | Delete draft or archived post |
| `POST` | `/api/posts/{id}/tags` | Bearer | Add tag. Body: `{ "tagId": "uuid" }` |
| `DELETE` | `/api/posts/{id}/tags/{tagId}` | Bearer | Remove tag from post |

### Post states

`Draft` → `Published` → `Archived`. See [posts/README.md](../domain/posts/README.md).

### Content field

`content` is a **ProseMirror JSON string** (TipTap output), not HTML.

---

## Tags

| Method | Path | Auth | Description |
|:---|:---|:---|:---|
| `GET` | `/api/tags` | Public | All tags with post counts |
| `POST` | `/api/tags` | Bearer | Create tag. Body: `{ "name": "..." }` |
| `PUT` | `/api/tags/{id}` | Bearer | Rename tag. Body: `{ "name": "..." }` |
| `DELETE` | `/api/tags/{id}` | Bearer | Delete tag |

---

## Response shapes (summary)

Key DTOs (camelCase in JSON):

**PostSummaryResult** — list items: `postId`, `title`, `slug`, `excerpt`, `coverImageUrl`, `authorDisplayName`, `postState`, `createdAt`, `publishedAt`, `tags[]`

**PostDetailResult** — single post: above plus `content`

**TagResult** — `tagId`, `name`, `slug`, `postCount`

**TagSummaryResult** — embedded on posts: `tagId`, `name`, `slug`

**PagedResult** — `items`, `totalCount`, `pageNumber`, `pageSize`, `hasNextPage`, …

Full schemas: `packages/api-types/src/api.d.ts` or `/openapi/v1.json`.

---

## Errors

Problem Details (`application/problem+json`) for validation and domain errors:

| HTTP | Typical cause |
|:---|:---|
| 400 | Validation failure |
| 401 | Missing or invalid JWT |
| 404 | Post or tag not found |
| 409 | Domain conflict (e.g. already published, duplicate tag name) |

Exception mapping: `standards/docs/conventions/backend/06-exception-hierarchy.md`.

---

## Frontend usage

Server Components call the API through typed client:

```typescript
import { getApiClient } from "@/lib/api/client";

const client = await getApiClient();
const { data, error } = await client.GET("/api/posts", {
  params: { query: { page: 1, pageSize: 10 } },
});
```

Admin client mutations from the browser go through **`/api-proxy/*`**, which mints a JWT and forwards to the API.

---

## Regenerating OpenAPI types

```bash
pnpm generate:api-types
```

Requires API running at default URL or uses committed `packages/api-types/openapi.json`.

Integration test `OpenApiExportTests` can refresh the committed spec from a running WebApi factory.

---

## Related

- [API README](../../apps/api/README.md)
- [Domain map](../domain/README.md)
- [Architecture](architecture.md)
