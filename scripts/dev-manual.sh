#!/usr/bin/env bash
# Manual dev stack: docker-compose Postgres + API. Start frontends in separate terminals.
set -euo pipefail

ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
cd "$ROOT"

echo "Starting docker-compose Postgres on port 5433..."
docker compose up -d

echo "Applying migrations..."
dotnet tool restore
dotnet ef database update \
  --project apps/api/src/LitePress.Infrastructure \
  --startup-project apps/api/src/LitePress.WebApi

echo "Starting API (Ctrl+C to stop)..."
echo "In other terminals: pnpm dev:web and pnpm dev:admin"
dotnet run --project apps/api/src/LitePress.WebApi
