#!/usr/bin/env bash
# Manual dev stack: docker-compose PostgreSQL + API. Start frontends in separate terminals.
set -euo pipefail

ROOT="$(cd "$(dirname "$0")/.." && pwd)"
cd "$ROOT"

echo "Starting docker-compose PostgreSQL on port 5432..."
docker compose up -d

echo "Applying Marten storage schema..."
export ASPNETCORE_ENVIRONMENT=Development
dotnet run --project apps/api/src/LitePress.WebApi -- --apply-schema-only

echo "Starting API (Ctrl+C to stop)..."
echo "In other terminals: pnpm dev:web and pnpm dev:admin"
dotnet run --project apps/api/src/LitePress.WebApi
