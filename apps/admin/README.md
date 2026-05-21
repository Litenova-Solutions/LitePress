# LiteNova Blog — Admin Panel

Next.js 15 (App Router) admin dashboard for managing blog posts and tags. Protected by Auth.js v5 with GitHub OAuth.

## Tech Stack

- **Next.js 15** with App Router
- **Auth.js v5** (GitHub OAuth, JWT session)
- **TipTap** rich-text editor
- **Tailwind CSS v4** + shadcn/ui

## Running

### With Aspire (recommended)

Start from `apps/api/`:

```bash
dotnet run --project src/LiteNova.Blog.AppHost
```

The admin app starts automatically. Check the Aspire dashboard (`https://localhost:15888`) for the URL. The admin still needs GitHub OAuth environment variables set locally (see below).

### Standalone

```bash
cd apps/admin
pnpm dev
# → http://localhost:3002
```

## Environment Variables

Create `apps/admin/.env.local` (never commit this file):

```env
# API
NEXT_PUBLIC_API_URL=http://localhost:5000
API_JWT_SECRET=dev-secret-key-must-be-at-least-32-characters-long!

# Auth.js
AUTH_SECRET=<generate with: openssl rand -base64 32>
AUTH_GITHUB_ID=<your GitHub OAuth App client ID>
AUTH_GITHUB_SECRET=<your GitHub OAuth App client secret>

# Access control — only this GitHub user ID may sign in
GITHUB_OWNER_ID=<your numeric GitHub user ID>
```

> **JWT secret alignment** — `API_JWT_SECRET` must match the API's `JwtSettings__Secret` (default dev value is `dev-secret-key-must-be-at-least-32-characters-long!`).

### Getting your GitHub user ID

```bash
curl https://api.github.com/users/<your-username> | grep '"id"'
```

### Setting up a GitHub OAuth App

1. Go to **GitHub → Settings → Developer settings → OAuth Apps → New OAuth App**.
2. Set **Homepage URL**: `http://localhost:3002`
3. Set **Authorization callback URL**: `http://localhost:3002/api/auth/callback/github`
4. Copy the **Client ID** and generate a **Client Secret**.

## Debugging

1. Set up `.env.local` as above.
2. Run `pnpm dev` — hot reload is active.
3. To attach a debugger:

```bash
NODE_OPTIONS='--inspect' pnpm dev
```

Then attach VS Code using the **Node.js: Attach** config on port `9229`.

### Debugging auth issues

- Check `AUTH_SECRET` is set (random base64 string).
- Confirm `GITHUB_OWNER_ID` matches your actual GitHub numeric ID.
- Confirm the OAuth callback URL registered in GitHub matches your running URL exactly.
- View session state in the browser: `http://localhost:3002/api/auth/session`.

## Admin Routes

| Route | Description |
|-------|-------------|
| `/` | Dashboard — recent posts |
| `/posts` | Post list |
| `/posts/new` | Create a new post |
| `/posts/[id]/edit` | Edit a post |
| `/tags` | Tag list |

All routes except `/api/auth/*` require an active session. Unauthenticated requests redirect to the GitHub OAuth sign-in page.

## Building for Production

```bash
pnpm build
pnpm start
```

Set all environment variables (including `AUTH_SECRET`, `AUTH_GITHUB_ID`, `AUTH_GITHUB_SECRET`) via your hosting platform's secret management. Never bake them into the image.
