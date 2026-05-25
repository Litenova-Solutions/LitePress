# Development Guide

How to clone, run, test, and debug LitePress locally.

---

## Which path am I on?

| Goal | Path | Postgres |
|:---|:---|:---|
| Full stack, daily work | **Aspire** (`pnpm dev:aspire`) | Aspire container (dynamic port) |
| Debug API only | **Manual** (`scripts/dev-manual.*`) | docker compose port **5433** |
| Debug one frontend | Manual API + `pnpm dev:web` or `dev:admin` | docker compose port **5433** |
| Frontends only | `pnpm dev` | Requires API + Postgres already running |
| Local E2E | `pwsh scripts/e2e-local.ps1` | docker compose port **5433** |

Do **not** run `docker compose up` and Aspire in the same session unless you know which connection string each command uses.

---

## Clone and bootstrap

```powershell
pwsh scripts/bootstrap.ps1
```

Linux/macOS:

```bash
bash scripts/bootstrap.sh
```

The script initializes the `standards/` submodule, restores pinned `dotnet-ef`, and runs `pnpm install`.

The `standards/` directory is a git submodule pointing at [Engineering-Standards](https://github.com/Litenova-Solutions/Engineering-Standards). Do not edit it from the LitePress repo.

---

## Path A: Aspire (recommended)

Starts Postgres, API, web, and admin with dynamic ports and injected env vars.

```bash
pnpm dev:aspire
```

Open the Aspire dashboard (usually `https://localhost:15888`) for service URLs and logs.

**Migrations:** Applied automatically when the API starts in Development. See [local-dev-migrations ADR](../decisions/local-dev-migrations.md).

**Dependencies:** Run `pwsh scripts/bootstrap.ps1` before the first `pnpm dev:aspire`. Bootstrap runs `pnpm install` at the repo root. The AppHost uses `WithPnpm(install: false)` so Aspire does not spawn a `*-installer` resource (broken on Windows when pnpm is a `.cmd` shim; see [dotnet/aspire#14880](https://github.com/microsoft/aspire/issues/14880)).

**Admin OAuth:** Copy `apps/admin/.env.example` to `.env.local`, or set AppHost user secrets for `auth-github-id`, `auth-github-secret`, `auth-secret`, and `github-owner-id`.

**Dev certificate:** Run `dotnet dev-certs https --trust` once to clear Aspire dashboard warnings about untrusted HTTPS.

---

## Database reset

If migrations were recreated and the API fails with errors like `relation "authors" already exists`, the Postgres volume still holds the old schema.

| Path | Command | Then |
|:---|:---|:---|
| Aspire | `pnpm db:reset:aspire` (`pnpm dev:stop` first) | `pnpm dev:aspire` — API migrates on startup |
| Manual | `pnpm db:reset` | Start API with `dev-manual` or `pnpm dev:api` |

Scripts: `scripts/db-reset.ps1` / `scripts/db-reset.sh` (pass `-Aspire` or `--aspire` for the Aspire volume).

---

## Path B: Manual services

For layer-isolated debugging with fixed ports.

### 1. Postgres + API

```powershell
pwsh scripts/dev-manual.ps1
```

```bash
bash scripts/dev-manual.sh
```

Uses docker compose Postgres on port **5433**:

```
Host=localhost;Port=5433;Database=litepress;Username=litepress;Password=litepress
```

### 2. Frontends (separate terminals)

```bash
pnpm dev:web      # http://localhost:3000
pnpm dev:admin    # http://localhost:3002
```

Admin requires `.env.local`; see [environment.md](environment.md).

### 3. Frontends only via Turbo

```bash
pnpm dev
```

Requires Postgres and API already running.

---

## Database migrations

### Apply (manual / CI path)

```bash
pnpm db:migrate
```

Or:

```bash
dotnet tool restore
dotnet ef database update \
  --project apps/api/src/LitePress.Infrastructure \
  --startup-project apps/api/src/LitePress.WebApi
```

### Add after schema changes

```bash
dotnet tool restore
dotnet ef migrations add <Name> \
  --project apps/api/src/LitePress.Infrastructure \
  --startup-project apps/api/src/LitePress.WebApi
```

Convert generated migration files to file-scoped namespaces (IDE0161).

### Reset (drop volume and re-apply)

Use after squashing or resetting migrations when the local database still has old tables.

```bash
# Aspire (pnpm dev:stop first)
pnpm db:reset:aspire
pnpm dev:aspire

# Manual path (docker compose on port 5433)
pnpm db:reset
```

---

## Admin first-time setup

1. Create a [GitHub OAuth App](https://github.com/settings/developers) with callback `http://localhost:3002/api/auth/callback/github`.
2. Get your numeric GitHub user ID: `curl https://api.github.com/users/<username>` → `"id"`.
3. Copy and edit the env file:

```bash
cp apps/admin/.env.example apps/admin/.env.local
```

4. Sign in at http://localhost:3002/login.

---

## API reference (local)

When the API runs in Development:

| Resource | URL (manual path) |
|:---|:---|
| OpenAPI JSON | http://localhost:5000/openapi/v1.json |
| Scalar UI | http://localhost:5000/scalar/v1 |

With Aspire, use the API URL from the dashboard and append `/scalar/v1`. Scalar is disabled outside Development.

Regenerate TypeScript client types after API contract changes:

```bash
pnpm generate:api-types
```

See [apps/api/README.md](../../apps/api/README.md) for details.

---

## Frontend UI (shadcn/ui)

LitePress uses shadcn/ui as the default UI in **every** frontend under `apps/`. Components live in each app's `components/ui/`. Shared design tokens live in `@litepress/config-tailwind/theme.css` (see [packages/config-tailwind/README.md](../../packages/config-tailwind/README.md)).

Bootstrap verifies that each Next.js app has Tailwind + shadcn scaffolding. After clone, committed component files are sufficient; re-run bootstrap after pulling large UI changes.

To add a component:

```bash
cd apps/admin   # or apps/web
npx shadcn@latest add <component-name>
```

Each app requires:

- `postcss.config.mjs` with `@tailwindcss/postcss`
- `app/globals.css` with `@import "tailwindcss"`, `@source` directives for `app/`, `components/`, and `features/`
- `components.json` from `npx shadcn@latest init`

Page composition and shadcn usage belong in `docs/ui/{app}/pages/*.md` and use-case docs § UI projection. Generic defaults are in `standards/docs/conventions/frontend/`.

---

## Verification gates

Run before opening a PR:

```bash
dotnet build apps/api/LitePress.slnx --configuration Release
dotnet test apps/api/LitePress.slnx --configuration Release --no-build
pnpm install --frozen-lockfile
pnpm lint && pnpm type-check && pnpm test && pnpm build
```

### Playwright E2E

```powershell
pwsh scripts/e2e-local.ps1
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

### VS Code / Cursor (committed configs)

1. Install recommended extensions (`.vscode/extensions.json`).
2. **F5 → Debug AppHost (Aspire)** for full-stack .NET debugging.
3. For Next.js: run with `NODE_OPTIONS='--inspect' pnpm dev:web`, then use **Aspire + Next.js debug** compound config.

Set breakpoints in command/query handlers, endpoints, or Next.js server components.

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
| Frontend change | `standards/docs/conventions/frontend/`, app `features/` folder |
| Agent work | [AGENTS.md](../../AGENTS.md) |

---

## Solution file

Use **`apps/api/LitePress.slnx`** only. Test projects:

- `LitePress.Domain.Tests`
- `LitePress.Application.Tests`
- `LitePress.Architecture.Tests`
- `LitePress.Integration.Tests`
