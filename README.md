# LiteNova Blog

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](./LICENSE)

LiteNova Blog is an open-source personal developer blog built with Next.js 15, ASP.NET Core 9, PostgreSQL, and LiteBus.

## Tech Stack

| Layer | Technology |
|-------|-----------|
| Monorepo | Turborepo + pnpm workspaces |
| Public website | Next.js 15 (App Router) |
| Admin panel | Next.js 15 (App Router) |
| API | ASP.NET Core 9 Minimal API |
| Database | PostgreSQL 17 + EF Core 9 |
| Messaging | [LiteBus](https://github.com/litenova/LiteBus) (CQRS/event mediator) |
| Mapping | Mapster |
| Validation | Ardalis.GuardClauses |
| Storage | Cloudflare R2 |
| Styling | Tailwind CSS v4 + shadcn/ui |
| Analytics | Umami |

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9)
- [Node.js 20+](https://nodejs.org/) and [pnpm](https://pnpm.io/)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (for PostgreSQL)

## Local Development

### 1. Clone and configure

```bash
git clone https://github.com/litenova/Blog.git
cd Blog
cp .env.example .env
```

Edit `.env` and fill in any required values (at minimum the database URL is pre-configured for local Docker).

### 2. Start infrastructure

```bash
docker compose up -d
```

This starts PostgreSQL on port `5432` and Umami analytics on port `3001`.

### 3. Apply database migrations

```bash
cd apps/api
dotnet ef database update --project src/LiteNova.Blog.Infrastructure --startup-project src/LiteNova.Blog.Api
```

### 4. Install front-end dependencies

```bash
cd ../../   # back to repo root
pnpm install
```

### 5. Run the full stack

Run all apps simultaneously with Turborepo:

```bash
pnpm dev
```

Or run individual apps:

```bash
# API only
cd apps/api
dotnet run --project src/LiteNova.Blog.Api

# Web (public blog) only
cd apps/web
pnpm dev

# Admin panel only
cd apps/admin
pnpm dev
```

| App | Default URL |
|-----|-------------|
| Public web | http://localhost:3000 |
| Admin panel | http://localhost:3002 |
| API | http://localhost:5000 |
| Umami | http://localhost:3001 |

### 6. Run tests

```bash
# .NET API tests
dotnet test apps/api/LiteNova.Blog.sln

# Front-end
pnpm turbo build --filter=web --filter=admin
```

## Debugging

### API (VS Code)

1. Open the repo in VS Code.
2. Press **F5** or use the **Run and Debug** panel (`Ctrl+Shift+D`).
3. Select **"Launch API"** from the dropdown (uses `.vscode/launch.json`).
4. Set breakpoints in any `.cs` file — the debugger attaches automatically.

### API (Visual Studio / Rider)

1. Open `apps/api/LiteNova.Blog.sln`.
2. Set `LiteNova.Blog.Api` as the startup project.
3. Press **F5** to launch with the debugger.

### API (CLI hot-reload)

```bash
cd apps/api
dotnet watch run --project src/LiteNova.Blog.Api
```

### Front-end (Next.js)

The Next.js dev server includes fast-refresh and source maps by default. Open Chrome DevTools or VS Code's **JavaScript Debugger** and attach to `http://localhost:3000` (or `3002` for admin).

## Architecture

```
apps/
  api/          ASP.NET Core 9 — Domain, Application (CQRS via LiteBus), Infrastructure, API
  web/          Next.js 15 — public-facing blog
  admin/        Next.js 15 — admin panel for managing posts and tags
packages/       Shared packages (UI components, TypeScript configs, etc.)
```

The API follows Clean Architecture with a CQRS pattern:

- **Domain** — Aggregate roots (`Post`, `Tag`), domain events (plain records), value objects
- **Application** — Use cases grouped by feature (e.g. `Posts/CreatePost/`), LiteBus handlers, validators, post-handlers for UoW and domain event dispatch
- **Infrastructure** — EF Core `BlogDbContext`, Cloudflare R2 storage service
- **API** — Minimal API endpoints grouped by feature (e.g. `Endpoints/Posts/CreatePost/`), Mapster mappings, middleware
