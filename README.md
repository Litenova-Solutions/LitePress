# LitePress

[![License: PolyForm Noncommercial](https://img.shields.io/badge/License-PolyForm%20Noncommercial-blue.svg)](./LICENSE)

Publishing stack from [Litenova Solutions](https://litenova.solutions): public reading site, private authoring dashboard, and .NET API backed by PostgreSQL. Reference implementation of [Litenova Engineering Standards](https://github.com/Litenova-Solutions/Engineering-Standards).

| Repository | Purpose |
|:---|:---|
| **[Litenova-Solutions/LitePress](https://github.com/Litenova-Solutions/LitePress)** (this repo) | LitePress application monorepo |
| **[Litenova-Solutions/Engineering-Standards](https://github.com/Litenova-Solutions/Engineering-Standards)** | Shared conventions, ADRs, and agent contracts (`standards/` submodule) |

---

## License (read this first)

| Audience | Terms |
|:---|:---|
| **Personal / noncommercial use** | Free under [PolyForm Noncommercial](./LICENSE) — hobby blogs, learning, forks for personal use |
| **Companies & commercial use** | Requires a [commercial license](./COMMERCIAL-LICENSE.md) — contact [Litenova Solutions](https://litenova.solutions) |

Details: [docs/decisions/licensing.md](docs/decisions/licensing.md).

---

## What you get

- **Public web** (`apps/web`) — SEO-first site: home, post pages, tags, Giscus comments
- **Admin** (`apps/admin`) — GitHub OAuth dashboard: TipTap editor, publish, tag management
- **API** (`apps/api`) — ASP.NET Core 10, CQRS via LiteBus, OpenAPI, JWT auth
- **Docs** — [How LitePress works](docs/how-it-works.md) · [Technical guide](docs/technical/README.md)

---

## Tech stack

| Layer | Technology |
|:---|:---|
| Monorepo | Turborepo + pnpm workspaces |
| Public web | Next.js 16 · React 19.2 · shadcn/ui · Tailwind 4 |
| Admin | Next.js 16 · Auth.js v5 · TipTap · shadcn/ui |
| API | ASP.NET Core 10 Minimal API · LiteBus CQRS · OpenAPI + Scalar (dev) |
| Database | PostgreSQL 17 · EF Core 10 (snake_case) |
| API types | OpenAPI → `@litepress/api-types` + `openapi-fetch` client |
| API docs (local) | Scalar at `/scalar/v1` (Development) · JSON at `/openapi/v1.json` |
| Local orchestration | .NET Aspire AppHost |
| CI | GitHub Actions (build, test, E2E publish flow) |

Engineering rules live in the [`standards/`](standards/) submodule. Domain docs are under [`docs/domain/`](docs/domain/).

---

## Repository structure

```
LitePress/
├── apps/
│   ├── api/          # .NET solution + Aspire AppHost
│   ├── web/          # Public Next.js app (port 3000)
│   └── admin/        # Admin Next.js app (port 3002)
├── packages/         # Shared TS: api-types, api-client, config-tailwind (theme CSS)
├── scripts/          # Bootstrap, dev helpers, db reset, E2E
├── standards/        # Engineering Standards submodule
└── docs/
    ├── how-it-works.md
    ├── technical/
    ├── features/
    └── decisions/
```

---

## Prerequisites

| Tool | Version | Notes |
|:---|:---|:---|
| [.NET SDK](https://dotnet.microsoft.com/download/dotnet/10.0) | 10.0+ | API and Aspire |
| [Node.js](https://nodejs.org/) | 22+ | Frontends |
| [pnpm](https://pnpm.io/installation) | 10+ | Workspace package manager |
| [Docker](https://www.docker.com/products/docker-desktop/) | any | Manual/E2E Postgres only; Aspire also uses Docker for its Postgres container |
| Aspire workload | — | `dotnet workload install aspire` (one-time) |

---

## First-time setup

Run these steps once after cloning.

### 1. Bootstrap the repo

```powershell
pwsh scripts/bootstrap.ps1
```

Linux/macOS:

```bash
bash scripts/bootstrap.sh
```

Bootstrap does the following:

| Step | What it does |
|:---|:---|
| `git submodule update --init --recursive` | Pulls the `standards/` submodule |
| `dotnet tool restore` | Installs pinned `dotnet-ef` from `.config/dotnet-tools.json` |
| `pnpm install` | Installs all frontend and workspace dependencies |
| Copy example config files | Creates AppHost `launchSettings.json`, `appsettings.Development.json`, and `apps/admin/.env.local` from examples when missing |

### 2. Trust the .NET dev certificate (recommended)

Aspire and the API use HTTPS locally. Without a trusted cert, the dashboard shows a warning and browser requests may fail.

```bash
dotnet dev-certs https --trust
```

See [Aspire dev certificates](https://aka.ms/aspire/devcerts).

### 3. Configure admin GitHub OAuth

Admin sign-in needs a GitHub OAuth App. Bootstrap may have created `apps/admin/.env.local` from the example; edit it with real values:

```bash
cp apps/admin/.env.example apps/admin/.env.local   # if not already created
```

| Field | Local value |
|:---|:---|
| Homepage URL | `http://localhost:3002` |
| Callback URL | `http://localhost:3002/api/auth/callback/github` |

Set `AUTH_GITHUB_ID`, `AUTH_GITHUB_SECRET`, and your numeric `GITHUB_OWNER_ID` in `.env.local`. Full details: [Environment variables](docs/technical/environment.md#admin-appsadmin).

### 4. Start the stack

```bash
pnpm dev:aspire
```

Open the Aspire dashboard (typically `https://localhost:15888`) for service URLs and logs. Postgres, API, web, and admin start together. The API applies EF Core migrations automatically on startup in Development.

**API reference (Development):** open `{api-base-url}/scalar/v1` from the dashboard, or `http://localhost:5000/scalar/v1` on the manual path. OpenAPI JSON: `/openapi/v1.json`. See [scalar-api-docs ADR](docs/decisions/scalar-api-docs.md).

Do **not** run `docker compose up` in the same session; Aspire manages its own Postgres instance on a separate Docker volume.

---

## Day-to-day development (Aspire)

After first-time setup, daily work is:

```bash
pnpm dev:aspire
```

| What happens | Detail |
|:---|:---|
| Postgres | Aspire starts a container with volume `litepress-postgres-data` (data persists across restarts) |
| API | Starts after Postgres is healthy; runs pending migrations in Development |
| Web + Admin | Start after dependencies are ready; ports are assigned dynamically (see dashboard) |
| Frontends | Dependencies are installed by `bootstrap` (`pnpm install` at repo root). Aspire does not re-run `pnpm install` on each start (avoids a known Windows issue with `.cmd` shims) |

Stop the stack with `Ctrl+C`, then run `pnpm dev:stop` if containers or processes are still running (common on Windows).

### When to re-run bootstrap

Re-run `pwsh scripts/bootstrap.ps1` after:

- pulling changes that update `pnpm-lock.yaml` or `.config/dotnet-tools.json`
- cloning on a new machine
- initializing the repo for the first time

You do **not** need bootstrap before every `pnpm dev:aspire`.

---

## Manual path (debugging one layer)

Use when you want fixed ports or to debug without Aspire.

| Step | Command |
|:---|:---|
| Postgres + API | `pwsh scripts/dev-manual.ps1` (or `bash scripts/dev-manual.sh`) |
| Web (new terminal) | `pnpm dev:web` |
| Admin (new terminal) | `pnpm dev:admin` |

This path uses `docker compose` Postgres on port **5433**. See [Development guide](docs/technical/development.md).

---

## Scripts reference

| Script / command | When to use | What it does |
|:---|:---|:---|
| `pwsh scripts/bootstrap.ps1` | First clone; after lockfile/tool changes | Submodule init, `dotnet tool restore`, `pnpm install`, copy example configs |
| `pnpm dev:aspire` | Daily full-stack work | Runs Aspire AppHost: Postgres + API + web + admin |
| `pnpm dev:stop` | After Ctrl+C if stack still running | Stops AppHost/API/frontends and Aspire Postgres containers |
| `pwsh scripts/dev-manual.ps1` | Debug API or use fixed ports | `docker compose up` Postgres on 5433, apply migrations, start API |
| `pnpm dev:web` / `pnpm dev:admin` | Manual path frontends | Start one Next.js app (API must already run) |
| `pnpm dev` | Frontends only via Turbo | Both Next.js apps; requires API + Postgres already running |
| `pnpm db:migrate` | Manual path or CI | Apply pending EF Core migrations to the manual Postgres (port 5433) |
| `pnpm db:reset` | Stale schema on manual Postgres | Drop docker compose volume, recreate Postgres, apply migrations |
| `pnpm db:reset:aspire` | Stale schema on Aspire Postgres | Drop `litepress-postgres-data` volume; restart `pnpm dev:aspire` to migrate |
| `pwsh scripts/e2e-local.ps1` | Before PR / local E2E | Full E2E stack mirroring CI (docker compose + API + Playwright) |

Shell equivalents: `scripts/bootstrap.sh`, `scripts/dev-manual.sh`, `scripts/db-reset.sh`, `scripts/e2e-local.sh`.

---

## Database reset (after migration changes)

If EF migrations were recreated or squashed, an existing Postgres volume may still contain old tables. Symptoms include `relation "authors" already exists` on API startup.

**Aspire path** (your case):

1. Stop Aspire: `pnpm dev:stop` (or `Ctrl+C` then `pnpm dev:stop` if processes remain).
2. Reset the volume:

   ```bash
   pnpm db:reset:aspire
   ```

   Or: `pwsh scripts/db-reset.ps1 -Aspire` / `bash scripts/db-reset.sh --aspire`

3. Start again: `pnpm dev:aspire`. The API recreates the schema from scratch.

**Manual path** (`dev-manual` / docker compose):

```bash
pnpm db:reset
```

This runs `docker compose down -v`, starts Postgres, and applies migrations.

---

## Troubleshooting

### Aspire dashboard: `web-installer` / `admin-installer` failed (exit code -4058 / -4048)

Known Aspire issue on Windows when `pnpm` is installed via npm (a `.cmd` shim). LitePress works around this by running `pnpm install` in bootstrap and setting `WithPnpm(install: false)` in the AppHost. Re-run bootstrap if frontends fail with missing modules:

```powershell
pwsh scripts/bootstrap.ps1
pnpm dev:aspire
```

Upstream: [dotnet/aspire#14880](https://github.com/dotnet/aspire/issues/14880).

### API fails: `relation "authors" already exists`

The Postgres volume has an old schema. See [Database reset](#database-reset-after-migration-changes) above.

### Dashboard warnings: unsecured endpoint / no trusted dev certificate

The dashboard runs in unsecured mode for local dev (`ASPIRE_DASHBOARD_UNSECURED_ALLOW_ANONYMOUS=true` in AppHost `launchSettings.json`). Suppress banners with `DASHBOARD__OTLP__SUPPRESSUNSECUREDMESSAGE` and `DASHBOARD__MCP__SUPPRESSUNSECUREDMESSAGE` (included in the example config).

To require login instead, remove `ASPIRE_DASHBOARD_UNSECURED_ALLOW_ANONYMOUS` and the suppress vars; the dashboard will prompt for a token on first open.

For HTTPS warnings, run `dotnet dev-certs https --trust` once.

### Admin login fails

Check `apps/admin/.env.local` OAuth values and that the GitHub callback URL matches exactly. See [Admin first-time setup](docs/technical/development.md#admin-first-time-setup).

---

## Verification

```bash
dotnet build apps/api/LitePress.slnx --configuration Release
dotnet test apps/api/LitePress.slnx --configuration Release --no-build
pnpm install --frozen-lockfile
pnpm lint && pnpm type-check && pnpm test && pnpm build
pwsh scripts/e2e-local.ps1
```

E2E publish flow: [`.github/workflows/e2e.yml`](.github/workflows/e2e.yml).

---

## Documentation

| Audience | Start here |
|:---|:---|
| Readers & authors | [How LitePress works](docs/how-it-works.md) |
| Developers | [Technical documentation](docs/technical/README.md) · [Repository map](docs/technical/repository-map.md) |
| AI agents / contributors | [AGENTS.md](AGENTS.md) |
| Domain & use cases | [docs/domain/README.md](docs/domain/README.md) |
| Decisions | [docs/decisions/README.md](docs/decisions/README.md) |
| v1 scope | [docs/v1-release-notes.md](docs/v1-release-notes.md) |

Per-app READMEs: [API](apps/api/README.md) · [Web](apps/web/README.md) · [Admin](apps/admin/README.md)

---

## Contributing

1. Read [AGENTS.md](AGENTS.md) and the relevant convention under `standards/docs/conventions/`.
2. Update `docs/domain/` when behavior changes.
3. Run verification commands above.
4. By contributing, you agree your contributions are licensed under the same [PolyForm Noncommercial](./LICENSE) terms.

Propose changes to Engineering Standards in the [Engineering-Standards](https://github.com/Litenova-Solutions/Engineering-Standards) repository, not inside the `standards/` submodule from this repo.

---

## License

LitePress is licensed under the [PolyForm Noncommercial License 1.0.0](./LICENSE).

**Commercial use** by companies and organizations requires a separate agreement. See [COMMERCIAL-LICENSE.md](COMMERCIAL-LICENSE.md) and contact [Litenova Solutions](https://litenova.solutions).
