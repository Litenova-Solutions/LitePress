# Development Guide

How to clone, run, test, and debug LitePress locally.

---

## Clone and bootstrap

```bash
git clone https://github.com/Litenova-Solutions/LitePress.git
cd LitePress
cd LitePress
git submodule update --init --recursive
pnpm install
```

Windows:

```powershell
pwsh scripts/bootstrap.ps1
pnpm install
```

The `standards/` directory is a git submodule pointing at [Engineering-Standards](https://github.com/Litenova-Solutions/Engineering-Standards). Do not edit it from the Blog repo.

---

## Database

Start PostgreSQL:

```bash
docker compose up -d
```

Default connection (port **5433** on host):

```
Host=localhost;Port=5433;Database=litepress;Username=litepress;Password=litepress
```

Apply migrations:

```bash
dotnet ef database update \
  --project apps/api/src/LiteNova.LitePress.Infrastructure \
  --startup-project apps/api/src/LiteNova.LitePress.WebApi
```

Add a migration after domain/schema changes:

```bash
dotnet ef migrations add <Name> \
  --project apps/api/src/LiteNova.LitePress.Infrastructure \
  --startup-project apps/api/src/LiteNova.LitePress.WebApi
```

---

## Run modes

### Option A: Aspire (all services)

```bash
dotnet run --project apps/api/src/LiteNova.LitePress.AppHost
```

Aspire starts PostgreSQL, API, web, and admin. Open the dashboard (usually `https://localhost:15888`) for URLs and logs.

Run migrations in a second terminal before first use if the database is empty.

### Option B: Individual services

Terminal 1 — Postgres (`docker compose up -d`) and API:

```bash
dotnet run --project apps/api/src/LiteNova.LitePress.WebApi
```

Terminal 2 — Web:

```bash
pnpm --filter web dev
```

Terminal 3 — Admin (requires `.env.local`; see [environment.md](environment.md)):

```bash
pnpm --filter admin dev
```

### Option C: Turborepo dev (frontends only)

```bash
pnpm dev
```

Requires PostgreSQL and API already running.

---

## Admin first-time setup

1. Create a [GitHub OAuth App](https://github.com/settings/developers) with callback `http://localhost:3002/api/auth/callback/github`.
2. Get your numeric GitHub user ID: `curl https://api.github.com/users/<username>` → `"id"`.
3. Create `apps/admin/.env.local`:

```env
API_URL=http://localhost:5000
API_JWT_SECRET=dev-secret-key-must-be-at-least-32-characters-long!
AUTH_SECRET=<openssl rand -base64 32>
AUTH_GITHUB_ID=<oauth client id>
AUTH_GITHUB_SECRET=<oauth client secret>
GITHUB_OWNER_ID=<your numeric id>
```

4. Sign in at http://localhost:3002/login.

---

## Verification gates

Run before opening a PR:

```bash
# Backend
dotnet build apps/api/LiteNova.LitePress.slnx --configuration Release
dotnet test apps/api/LiteNova.LitePress.slnx --configuration Release --no-build

# Frontend (from repo root)
pnpm install --frozen-lockfile
pnpm lint && pnpm type-check && pnpm test && pnpm build
```

### Playwright E2E

Requires Postgres, migrated API, and built web:

```bash
docker compose up -d
dotnet ef database update --project apps/api/src/LiteNova.LitePress.Infrastructure --startup-project apps/api/src/LiteNova.LitePress.WebApi
dotnet run --project apps/api/src/LiteNova.LitePress.WebApi &
pnpm --filter web build && pnpm --filter web start &
pnpm exec playwright test --config apps/web/playwright.config.ts
```

- `home page loads` — always runs
- `published post appears on home and slug page` — needs API seed (runs in CI; skips locally if API is down)

CI runs the full E2E stack in `.github/workflows/e2e.yml`.

---

## Regenerate API types

After changing API endpoints or DTOs:

```bash
# Start API, then:
pnpm generate:api-types
```

Commits `packages/api-types/openapi.json` and `packages/api-types/src/api.d.ts`.

---

## CI workflows

| Workflow | Triggers | Runs |
|:---|:---|:---|
| `api.yml` | `apps/api/**` | `dotnet build` + `dotnet test` |
| `web.yml` | `apps/web/**`, `packages/**` | lint, type-check, test, build |
| `admin.yml` | `apps/admin/**`, `packages/**` | lint, type-check, test, build |
| `e2e.yml` | web + api + packages | Postgres, migrate, API, Playwright publish flow |

---

## Debugging

### API (VS Code)

1. Install C# Dev Kit and Aspire extensions.
2. Debug `LiteNova.LitePress.AppHost` or `LiteNova.LitePress.WebApi`.
3. Set breakpoints in command/query handlers or endpoints.

### Next.js

```bash
NODE_OPTIONS='--inspect' pnpm --filter web dev
```

Attach VS Code **JavaScript Debug Terminal** or Node attach on port 9229.

### Auth issues (admin)

- Session: http://localhost:3002/api/auth/session
- Confirm `GITHUB_OWNER_ID` matches your GitHub numeric ID
- Callback URL in GitHub OAuth app must match exactly

---

## Project layout for contributors

| Task | Read first |
|:---|:---|
| New use case | `docs/domain/{feature}/{use-case}.md`, then `standards/docs/guides/add-new-use-case.md` |
| API change | Layer convention in `standards/docs/conventions/backend/` |
| Frontend change | `standards/docs/conventions/frontend/`, app `domain/` folder |
| Agent work | [AGENTS.md](../../AGENTS.md) |

---

## Solution file

Use **`apps/api/LiteNova.LitePress.slnx`** only. Test projects:

- `LiteNova.LitePress.Domain.Tests`
- `LiteNova.LitePress.Application.Tests`
- `LiteNova.LitePress.Architecture.Tests`
- `LiteNova.LitePress.Integration.Tests`
