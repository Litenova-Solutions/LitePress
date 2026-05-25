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
| Public web | Next.js 16 · React 19.2 · TypeScript 6 |
| Admin | Next.js 16 · Auth.js v5 · TipTap |
| API | ASP.NET Core 10 Minimal API · LiteBus CQRS |
| Database | PostgreSQL 17 · EF Core 10 (snake_case) |
| API types | OpenAPI → `@litepress/api-types` + `openapi-fetch` client |
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
├── packages/         # Shared TS: api-types, api-client, configs
├── standards/        # Engineering Standards submodule
└── docs/
    ├── how-it-works.md
    ├── technical/
    ├── domain/
    └── decisions/
```

---

## Prerequisites

| Tool | Version | Notes |
|:---|:---|:---|
| [.NET SDK](https://dotnet.microsoft.com/download/dotnet/10.0) | 10.0+ | API and Aspire |
| [Node.js](https://nodejs.org/) | 22+ | Frontends |
| [pnpm](https://pnpm.io/installation) | 10+ | Workspace package manager |
| [Docker](https://www.docker.com/products/docker-desktop/) | any | Manual/E2E Postgres only |
| Aspire workload | — | `dotnet workload install aspire` (one-time) |

---

## Quick start

### 1. Bootstrap

```powershell
pwsh scripts/bootstrap.ps1
```

Linux/macOS:

```bash
bash scripts/bootstrap.sh
```

This initializes the `standards/` submodule, runs `dotnet tool restore`, and `pnpm install`.

### 2. Run with Aspire (recommended)

```bash
pnpm dev:aspire
```

Aspire starts its own Postgres container, API, web, and admin. Migrations apply automatically in Development when the API starts. Open the Aspire dashboard (typically `https://localhost:15888`) for URLs and logs.

Do **not** run `docker compose up` in the same session; Aspire uses a separate Postgres instance.

### 3. Configure admin OAuth (first-time sign-in)

Copy the example env file and add your GitHub OAuth credentials:

```bash
cp apps/admin/.env.example apps/admin/.env.local
```

Or override AppHost parameters via user secrets. See [Environment variables](docs/technical/environment.md#admin-appsadmin).

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
