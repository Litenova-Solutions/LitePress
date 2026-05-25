# Create Post

| Field | Value |
|:---|:---|
| Feature | `posts` |
| Status | Active (v1 complete) |
| Last updated | 2026-05-25 |

---

## Summary

An authenticated Author creates a new Post in Draft state with title, ProseMirror JSON content, optional excerpt and cover image URL, and optional initial tag assignments. The API returns the new `PostId` and generated slug.

---

## Command

| Type | Name | Input | Output | Idempotency |
|:---|:---|:---|:---|:---:|
| Command | `CreatePostCommand` | `PostId`, `AuthorId` (from JWT), `Title`, `Content`, `Excerpt?`, `CoverImageUrl?`, `TagIds[]` | `CreatePostCommandResult` (`PostId`, `Slug`) | No |

### Structural validation

- Title: required, max 200 characters
- Content: required (ProseMirror JSON string)
- Excerpt: optional, max 500 characters

---

## Domain Behavior

- Factory `Post.Create()` sets Draft state, generates slug from title, assigns tags from `TagIds`.
- Raises `PostCreated`.

---

## Exceptions

| Exception | When | HTTP status |
|:---|:---:|---:|
| `PostTitleRequiredException` | Empty title | 400 |
| `PostTitleTooLongException` | Title > 200 chars | 400 |
| `PostContentRequiredException` | Empty content | 400 |
| `PostExcerptTooLongException` | Excerpt > 500 chars | 400 |
| `TagNotFoundException` | Invalid tag ID in `TagIds` | 404 |

---

## HTTP Endpoint

| Method | Path | Auth | Rate limit |
|:---|:---|:---|:---|
| POST | `/api/posts` | Bearer JWT | authenticated-api |

Request body: `{ title, content, excerpt?, coverImageUrl?, tagIds[] }`.

Returns 201 with `{ postId, slug }` and `Location: /api/posts/{postId}`.

---

## UI (admin)

### Route and entry

- Route: `app/(dashboard)/posts/new/page.tsx`
- Domain entry: `domain/posts/create/CreatePostForm.tsx`

### Components (shadcn/ui)

Use local imports from `@/components/ui/`:

| UI element | Component |
|:---|:---|
| Form shell | `Card`, `CardHeader`, `CardTitle`, `CardDescription`, `CardContent` |
| Fields | `Label`, `Input`, `Textarea` |
| Submit | `Button` |
| API errors | `Alert`, `AlertTitle`, `AlertDescription` |
| Success / failure feedback | `sonner` toast via `Toaster` in root layout |
| Rich text | `TipTapEditor` (TipTap; admin-specific) |

Shared theme: `@litepress/config-tailwind/theme.css`.

### States

| State | Behavior |
|:---|:---|
| Loading | Submit button disabled during client mutation |
| Empty | Blank form with TipTap editor |
| Error | Inline `Alert` and error toast on API failure |
| Loaded | Redirect to post edit page on success |

### Mutations

Client mutation via `/api-proxy/posts` with JSON body. TipTap outputs ProseMirror JSON for `content`. (Server Action + Zod in the action file remains the standards default for new forms; this use case currently uses the api-proxy client pattern documented here.)

---

## Acceptance Criteria

1. Given an authenticated Author, when they submit a valid title and content, then a Draft post is created and they are redirected to the edit page. (Playwright)
2. Given an empty title, when they submit, then validation fails before the API call. (Vitest)
3. Given valid input, when the API receives the command, then a Draft post exists with the correct `AuthorId`. (Domain + Integration)

---

## Out of Scope

Publishing, scheduled publishing, cover image upload to R2.
