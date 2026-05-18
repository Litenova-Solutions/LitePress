# LiteNova Blog

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](./LICENSE)

LiteNova Blog is an open-source personal developer blog built with **Next.js 15**, **ASP.NET Core 9**, **PostgreSQL**, and **LiteBus**.

## Tech Stack

- Turborepo + pnpm workspaces
- Next.js 15 (App Router, TypeScript)
- ASP.NET Core 9 Minimal API
- Entity Framework Core 9 + Npgsql
- LiteBus (commands, queries, events)
- Tailwind CSS v4 + shadcn/ui
- PostgreSQL + Umami (local docker-compose)

## Monorepo Structure

- `apps/web` - Public blog app
- `apps/admin` - Admin dashboard app
- `apps/api` - .NET API solution (`LiteNova.Blog.sln`)
- `packages/ui` - Shared UI package
- `packages/config-*` - Shared TS/ESLint/Tailwind configs

## Prerequisites

- Node.js 22+
- pnpm 10+
- .NET SDK 9
- Docker + Docker Compose

## Environment Setup

1. Clone the repository.
2. Copy env file:
   ```bash
   cp .env.example .env
   ```
3. Fill required variables in `.env`.

## Run the Full Stack Locally

1. Start infrastructure services:
   ```bash
   docker compose up -d
   ```
2. Install workspace dependencies:
   ```bash
   pnpm install
   ```
3. Start web/admin apps:
   ```bash
   pnpm dev
   ```
4. In a second terminal, start the API:
   ```bash
   dotnet run --project apps/api/src/LiteNova.Blog.Api/LiteNova.Blog.Api.csproj
   ```

Default local ports:

- Web: `http://localhost:3000`
- Admin: `http://localhost:3002`
- API: `http://localhost:5000` (or configured ASP.NET Core URL)
- PostgreSQL: `localhost:5432`
- Umami: `http://localhost:3001`

## Debugging

### Debugging API (.NET)

- Build and run tests:
  ```bash
  dotnet build apps/api/LiteNova.Blog.sln
  dotnet test apps/api/LiteNova.Blog.sln
  ```
- Run API with verbose output:
  ```bash
  dotnet run --project apps/api/src/LiteNova.Blog.Api/LiteNova.Blog.Api.csproj
  ```

### Debugging Next.js Apps

- Validate production builds:
  ```bash
  pnpm turbo build --filter=web --filter=admin
  ```
- Run one app only:
  ```bash
  pnpm --filter web dev
  pnpm --filter admin dev
  ```

## CI Workflows

- `web.yml` builds `apps/web`
- `admin.yml` builds `apps/admin`
- `api.yml` restores/builds/tests `apps/api`
