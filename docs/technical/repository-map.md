# Repository Map

Where important files live in the LitePress monorepo and what each one is for. Use this when onboarding or when you are unsure which file to edit.

For how to run the stack, see [Development guide](development.md). For env vars, see [Environment variables](environment.md).

---

## Start here

| If you want to… | Open |
|:---|:---|
| Run the full stack | `pnpm dev:aspire` — see [Development guide](development.md#path-a-aspire-recommended) |
| First-time machine setup | `scripts/bootstrap.ps1` or `scripts/bootstrap.sh` |
| Find a command quickly | [AGENTS.md](../../AGENTS.md#commands) or root `package.json` `scripts` |
| Understand a domain feature | `docs/domain/` |
| Change engineering rules | [Engineering Standards](https://github.com/Litenova-Solutions/Engineering-Standards) (`standards/` submodule), not edits inside LitePress |

---

## Repository root

| Path | Purpose |
|:---|:---|
| `package.json` | Root pnpm scripts: `dev:aspire`, `dev:api`, `dev:web`, `dev:admin`, `db:migrate`, Turbo gates (`lint`, `build`, …) |
| `pnpm-workspace.yaml` | Defines workspace packages (`apps/*`, `packages/*`) |
| `turbo.json` | Turborepo task graph for build, dev, lint, test |
| `docker-compose.yml` | **Manual / E2E path only.** Postgres on port **5433**. Not used with Aspire AppHost |
| `AGENTS.md` | Agent and contributor contract: read order, rules, commands |
| `README.md` | Project overview and quick start |
| `.gitignore` | Ignores build cache, secrets, test output (see [Ignored locally](#ignored-locally-not-in-git)) |
| `.config/dotnet-tools.json` | Pins `dotnet-ef` version; run `dotnet tool restore` after clone |
| `.vscode/` | Committed IDE configs: debug AppHost, attach Node, tasks, recommended extensions |
| `.github/workflows/` | CI: `api.yml`, `web.yml`, `admin.yml`, `e2e.yml` |

---

## `scripts/`

| Script | Purpose |
|:---|:---|
| `bootstrap.ps1` / `bootstrap.sh` | After clone: init `standards/` submodule, `dotnet tool restore`, `pnpm install`, prerequisite checks |
| `dev-manual.ps1` / `dev-manual.sh` | Manual path: `docker compose up`, migrate, run API (fixed port 5000). Start frontends separately |
| `e2e-local.ps1` / `e2e-local.sh` | Local Playwright stack mirroring `.github/workflows/e2e.yml` |

---

## `apps/api/` (.NET solution)

| Path | Purpose |
|:---|:---|
| `LitePress.slnx` | Solution file — use this only |
| `global.json` | Pinned .NET SDK version |
| `Directory.Build.props` | Shared MSBuild settings (nullable, warnings as errors, IDE0161) |
| `Directory.Packages.props` | Central NuGet package versions |
| `src/LitePress.AppHost/` | **Aspire entry point.** Starts Postgres, API, web, admin; injects URLs and secrets |
| `src/LitePress.AppHost/appsettings.Development.json` | Local AppHost parameter defaults (gitignored; copy from docs or use user secrets) |
| `src/LitePress.ServiceDefaults/` | Shared OTel, health checks (`/health`, `/alive`), HTTP resilience |
| `src/LitePress.WebApi/` | ASP.NET Core API: endpoints, middleware, OpenAPI, Scalar (dev) |
| `src/LitePress.WebApi/appsettings.json` | Committed defaults (connection string for manual path, CORS, JWT placeholder) |
| `src/LitePress.WebApi/appsettings.Development.json` | Local API overrides (gitignored) |
| `src/LitePress.Infrastructure/` | EF Core, repositories, `DatabaseMigrationExtensions` (dev auto-migrate) |
| `src/LitePress.Domain/` | Aggregates, value objects, domain events |
| `src/LitePress.Application.*` | CQRS handlers (Write, Read, Reactions) |
| `tests/` | Unit, architecture, integration tests |

**Run:** `pnpm dev:aspire` (from root) or `dotnet run --project apps/api/src/LitePress.AppHost`

**Migrations:** Auto-applied in Development on API startup (Aspire path). Manual: `pnpm db:migrate`. See [local-dev-migrations ADR](../decisions/local-dev-migrations.md).

---

## `apps/web/` (public Next.js)

| Path | Purpose |
|:---|:---|
| `package.json` | App scripts; dev server port **3000** |
| `.env.example` | Template for optional overrides — copy to `.env.local` |
| `lib/env.ts` | Validated server/public env vars |
| `features/` | Feature UI aligned with `docs/domain/` |
| `components/ui/` | shadcn/ui components (owned per app) |
| `postcss.config.mjs` | Tailwind v4 PostCSS entry |
| `e2e/` | Playwright tests; `.seed.json` is generated locally (gitignored) |
| `playwright.config.ts` | E2E config |

**Run:** `pnpm dev:web` (requires API for dynamic data)

---

## `apps/admin/` (authoring Next.js)

| Path | Purpose |
|:---|:---|
| `package.json` | App scripts; dev server port **3002** |
| `.env.example` | GitHub OAuth template — copy to `.env.local` for sign-in |
| `lib/env.ts` | Auth.js and API env validation |
| `components/ui/` | shadcn/ui components (owned per app) |
| `postcss.config.mjs` | Tailwind v4 PostCSS entry |

**Run:** `pnpm dev:admin` (OAuth requires real GitHub app credentials)

---

## `packages/`

| Package | Purpose |
|:---|:---|
| `@litepress/api-types` | OpenAPI-generated TypeScript types |
| `@litepress/api-client` | `openapi-fetch` client wrapper |
| `@litepress/config-eslint` | Shared ESLint config |
| `@litepress/config-typescript` | Shared `tsconfig` base |
| `@litepress/config-tailwind` | Shared shadcn theme CSS tokens (not React components) |

Regenerate types after API changes: `pnpm generate:api-types`

---

## `docs/`

| Path | Purpose |
|:---|:---|
| `how-it-works.md` | Non-technical product guide |
| `technical/` | Developer docs (this folder) |
| `technical/development.md` | Clone, run modes, debug, verification |
| `technical/environment.md` | All env vars and OAuth setup |
| `technical/repository-map.md` | This file |
| `features/` | Ubiquitous language, use cases, acceptance criteria |
| `decisions/` | LitePress ADRs (auth, dual apps, migrations, deferrals, …) |

---

## `standards/` (submodule)

Git submodule pointing at [Engineering-Standards](https://github.com/Litenova-Solutions/Engineering-Standards). Shared conventions, blueprints, CI templates.

**Do not edit from LitePress.** Propose changes in the standards repository, then update the submodule (`git submodule update --remote standards`).

After clone: `git submodule update --init --recursive` (or `scripts/bootstrap.*`).

---

## Run modes (which files matter)

| Mode | Postgres | Entry command | Key config |
|:---|:---|:---|:---|
| **Aspire (default)** | Aspire container | `pnpm dev:aspire` | AppHost `Program.cs`, Aspire dashboard |
| **Manual** | `docker-compose.yml` :5433 | `scripts/dev-manual.*` + `pnpm dev:web/admin` | `appsettings.json`, `.env.local` |
| **Frontends only** | Already running | `pnpm dev` | API + Postgres must be up |
| **E2E local** | `docker-compose.yml` :5433 | `scripts/e2e-local.*` | Same env as `e2e.yml` |

Do not run Docker Compose Postgres and Aspire in the same session unless you know which connection string each tool uses.

---

## Ignored locally (not in git)

These are generated or secret. See root `.gitignore`.

| Pattern | Reason |
|:---|:---|
| `*.tsbuildinfo` | TypeScript incremental cache |
| `.next/`, `dist/`, `bin/`, `obj/` | Build output |
| `.turbo/`, `.eslintcache` | Tool cache |
| `.env`, `.env.local` | Secrets and local overrides |
| `appsettings.Development.json` | Local API/AppHost settings (may contain secrets) |
| `test-results/`, `playwright-report/` | Playwright output |
| `apps/web/e2e/.seed.json` | E2E seed data generated at test time |

**Committed env templates:** `apps/admin/.env.example`, `apps/web/.env.example` (no secrets).

---

## Related ADRs

| ADR | Topic |
|:---|:---|
| [local-dev-migrations.md](../decisions/local-dev-migrations.md) | Dev auto-migrate vs manual `pnpm db:migrate` |
| [dual-nextjs-apps.md](../decisions/dual-nextjs-apps.md) | Why web and admin are separate apps |
| [admin-auth.md](../decisions/admin-auth.md) | GitHub OAuth + JWT for API |
| [v1-scope-deferrals.md](../decisions/v1-scope-deferrals.md) | What is intentionally out of v1 scope |
