# LitePress — Admin

Next.js 16 authoring dashboard. GitHub OAuth (single owner), TipTap editor, post/tag CRUD via API proxy.

User guide: [docs/how-it-works.md](../../docs/how-it-works.md) · Auth: [docs/decisions/admin-auth.md](../../docs/decisions/admin-auth.md)

---

## Stack

- Next.js 16 · React 19.2 · TypeScript 6
- Auth.js v5 (GitHub OAuth)
- TipTap → ProseMirror JSON
- Server: `getApiClient()` with minted JWT
- Client mutations: `/api-proxy/*` route

---

## Run

### With Aspire

```bash
dotnet run --project apps/api/src/LiteNova.Blog.AppHost
```

You still need GitHub OAuth env vars locally (see below).

### Standalone

```bash
pnpm --filter admin dev
# → http://localhost:3002
```

---

## Environment

Create `apps/admin/.env.local`:

```env
API_URL=http://localhost:5000
API_JWT_SECRET=dev-secret-key-must-be-at-least-32-characters-long!
AUTH_SECRET=<openssl rand -base64 32>
AUTH_GITHUB_ID=<github oauth client id>
AUTH_GITHUB_SECRET=<github oauth client secret>
GITHUB_OWNER_ID=<your numeric github user id>
```

Full reference: [docs/technical/environment.md](../../docs/technical/environment.md).

### GitHub OAuth App

| Setting | Local value |
|:---|:---|
| Callback URL | `http://localhost:3002/api/auth/callback/github` |

Get your user ID: `curl https://api.github.com/users/<username>` → `"id"`.

---

## Routes

| Route | Description |
|:---|:---|
| `/login` | GitHub sign-in |
| `/` | Dashboard stats |
| `/posts` | Post list |
| `/posts/new` | Create draft |
| `/posts/[id]` | Edit / publish / archive / delete / tag assignment |
| `/tags` | Create, rename, delete tags |
| `/api/auth/[...nextauth]` | Auth.js handlers |
| `/api-proxy/[...path]` | Authenticated API proxy |

All dashboard routes require session except auth endpoints.

---

## Code layout

```
apps/admin/
├── app/                    # Routes and layouts
├── domain/                 # Feature UI by use case
│   ├── posts/create/
│   ├── posts/update/
│   └── tags/delete|rename/
├── lib/api/client.ts       # getApiClient() for Server Components / actions
└── lib/auth/mintApiToken.ts
```

---

## Build and test

```bash
pnpm --filter admin lint
pnpm --filter admin type-check
pnpm --filter admin build
```

---

## Debugging auth

- Session JSON: http://localhost:3002/api/auth/session
- Confirm `GITHUB_OWNER_ID` matches your GitHub numeric ID
- OAuth callback URL must match GitHub app settings exactly
- `API_JWT_SECRET` must match API `JwtSettings__Secret`
