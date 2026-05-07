# LiteNova Blog

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](./LICENSE)

LiteNova Blog is an open-source personal developer blog built with Next.js 15, ASP.NET Core 9, PostgreSQL, and LiteBus.

## Tech Stack

- Turborepo + pnpm workspaces
- Next.js 15 (web + admin)
- ASP.NET Core 9 Minimal API
- PostgreSQL + EF Core 9
- LiteBus (CQRS), Mapster, Ardalis.GuardClauses
- Tailwind CSS v4 + shadcn/ui

## Local Development

1. Clone the repository.
2. Copy environment variables:
   ```bash
   cp .env.example .env
   ```
3. Start local services:
   ```bash
   docker compose up -d
   ```
4. Install dependencies:
   ```bash
   pnpm install
   ```
5. Start the monorepo in dev mode:
   ```bash
   pnpm dev
   ```
