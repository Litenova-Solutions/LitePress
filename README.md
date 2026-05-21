# LiteNova Blog

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](./LICENSE)

LiteNova Blog is an open-source personal developer blog built with Next.js 15, ASP.NET Core 10, PostgreSQL, and LiteBus — orchestrated locally with .NET Aspire.

## Tech Stack

| Layer | Technology |
|-------|-----------|
| Monorepo | Turborepo + pnpm workspaces |
| Public website | Next.js 15 (App Router) |
| Admin panel | Next.js 15 (App Router, Auth.js v5) |
| API | ASP.NET Core 10 Minimal API |
| Database | PostgreSQL 17 + EF Core 10 |
| Messaging | [LiteBus](https://github.com/litenova/LiteBus) v4 (CQRS / event mediator) |
| Validation | Ardalis.GuardClauses |
| Styling | Tailwind CSS v4 + shadcn/ui |
| Dev orchestration | .NET Aspire 13 |

## Repository Structure

```
Blog/
├── apps/
│   ├── api/          # .NET 10 back-end (Aspire AppHost lives here too)
│   ├── web/          # Next.js public blog (port 3000)
│   └── admin/        # Next.js admin dashboard (port 3002)
├── packages/         # Shared TypeScript packages (UI, configs)
├── standards/        # Engineering standards (git submodule)
└── docs/             # Project-level documentation
```

## Prerequisites

| Tool | Version | Notes |
|------|---------|-------|
| [.NET SDK](https://dotnet.microsoft.com/download/dotnet/10.0) | 10.0+ | Required for API and Aspire |
| [Node.js](https://nodejs.org/) | 22+ | For Next.js frontends |
| [pnpm](https://pnpm.io/installation) | 9+ | Workspace package manager |
| [Docker Desktop](https://www.docker.com/products/docker-desktop/) | any | PostgreSQL container via Aspire |

> **Aspire workload** — Install once with: `dotnet workload install aspire`

## Quick Start — Aspire (recommended)

Aspire starts all services (PostgreSQL, API, web, admin) with a single command and provides a live dashboard.

### 1. Clone

```bash
git clone https://github.com/litenova/Blog.git
cd Blog
git submodule update --init
```

### 2. Install Node.js dependencies

```bash
pnpm install
```

### 3. Apply database migrations

Aspire starts PostgreSQL automatically, but you must run migrations once (or after schema changes):

```bash
cd apps/api
dotnet ef database update \
  --project src/LiteNova.Blog.Infrastructure \
  --startup-project src/LiteNova.Blog.WebApi \
  -- --environment Development
```

> **Connection string** — By default migrations connect to `localhost:5433`. Start the Aspire stack first (step 4), wait for PostgreSQL to be ready, then run migrations in a second terminal.

### 4. Run with Aspire

```bash
cd apps/api
dotnet run --project src/LiteNova.Blog.AppHost
```

The Aspire dashboard opens automatically at `https://localhost:15888`. From there you can navigate to:

| Service | URL |
|---------|-----|
| Aspire dashboard | https://localhost:15888 |
| Public web | http://localhost:3000 (dynamic — check dashboard) |
| Admin panel | http://localhost:3002 (dynamic — check dashboard) |
| API | http://localhost:5000 (dynamic — check dashboard) |

> Ports are dynamically allocated by Aspire. Check the dashboard **Resources** tab for the actual URLs.

## Running Services Individually (without Aspire)

Useful for debugging a single layer. Requires PostgreSQL running separately (Docker or local).

### Start PostgreSQL (Docker)

```bash
docker run -d \
  --name blog-postgres \
  -e POSTGRES_DB=blog \
  -e POSTGRES_USER=blog \
  -e POSTGRES_PASSWORD=blog \
  -p 5433:5432 \
  postgres:17
```

### API

```bash
cd apps/api
dotnet run --project src/LiteNova.Blog.WebApi
# → http://localhost:5000
```

### Public web

```bash
cd apps/web
pnpm dev
# → http://localhost:3000
```

### Admin panel

```bash
cd apps/admin
pnpm dev
# → http://localhost:3002
```

### All services via Turborepo

```bash
# From repo root — requires PostgreSQL already running
pnpm dev
```

## Debugging

See individual README files for per-project debug instructions:

- [apps/api/README.md](apps/api/README.md) — .NET debugger, EF migrations
- [apps/web/README.md](apps/web/README.md) — Next.js dev tools
- [apps/admin/README.md](apps/admin/README.md) — Admin panel, Auth.js, GitHub OAuth

### Debugging with Aspire + VS Code

1. Install the [C# Dev Kit](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit) and [Aspire](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.dotnet-aspire) extensions.
2. Open the repo root in VS Code.
3. Use **Run and Debug → Start Debugging** on the `LiteNova.Blog.AppHost` project.
4. Aspire starts all resources. The dashboard opens at `https://localhost:15888`.
5. Set breakpoints in the API — the debugger attaches automatically.

## Environment Variables

### API (`apps/api/src/LiteNova.Blog.WebApi/`)

| Variable | Default (dev) | Description |
|----------|---------------|-------------|
| `ConnectionStrings__Database` | `Host=localhost;Port=5433;...` | PostgreSQL connection string (injected by Aspire) |
| `JwtSettings__Secret` | `dev-secret-key-must-be-at-least-32-characters-long!` | JWT signing key — **change in production** |
| `Cors__WebOrigin` | `http://localhost:3000` | Allowed CORS origin for public web (injected by Aspire) |
| `Cors__AdminOrigin` | `http://localhost:3002` | Allowed CORS origin for admin (injected by Aspire) |

### Admin (`apps/admin/`)

| Variable | Default (dev) | Description |
|----------|---------------|-------------|
| `NEXT_PUBLIC_API_URL` | `http://localhost:5000` | API base URL (injected by Aspire) |
| `API_JWT_SECRET` | `dev-secret-key-must-be-at-least-32-characters-long!` | Must match API `JwtSettings__Secret` |
| `AUTH_SECRET` | — | Auth.js secret — generate with `openssl rand -base64 32` |
| `AUTH_GITHUB_ID` | — | GitHub OAuth App Client ID |
| `AUTH_GITHUB_SECRET` | — | GitHub OAuth App Client Secret |
| `GITHUB_OWNER_ID` | — | Your GitHub numeric user ID (only this user can sign in) |

### Public web (`apps/web/`)

| Variable | Default (dev) | Description |
|----------|---------------|-------------|
| `NEXT_PUBLIC_API_URL` | `http://localhost:5000` | API base URL (injected by Aspire) |

## Running Tests

```bash
# .NET unit/architecture tests
cd apps/api
dotnet test LiteNova.Blog.slnx

# Front-end lint
pnpm lint
```

## Contributing

See [AGENTS.md](AGENTS.md) for coding conventions and the agentic development guide.
