#!/usr/bin/env bash
# Drop the local Postgres data volume and re-apply EF Core migrations.
#
# Usage:
#   bash scripts/db-reset.sh              # manual path (docker compose, port 5433)
#   bash scripts/db-reset.sh --aspire     # Aspire path (litepress-postgres-data volume)
#
# Stop the running stack before resetting. For Aspire, stop pnpm dev:aspire first.
set -euo pipefail

ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
cd "$ROOT"

ASPIRE=false
if [[ "${1:-}" == "--aspire" ]]; then
  ASPIRE=true
fi

wait_for_postgres() {
  local max_attempts=30
  for ((i = 1; i <= max_attempts; i++)); do
    if docker compose ps postgres 2>/dev/null | grep -q "(healthy)"; then
      return 0
    fi
    sleep 2
  done
  echo "error: Postgres did not become healthy in time." >&2
  exit 1
}

if [[ "$ASPIRE" == true ]]; then
  echo "Stopping Aspire resources..."
  bash "$(dirname "${BASH_SOURCE[0]}")/dev-aspire-stop.sh"

  echo "Resetting Aspire Postgres volume (litepress-postgres-data)..."
  if ! docker volume rm litepress-postgres-data -f 2>&1; then
    echo "error: could not remove volume. Run pnpm dev:stop, then run this script again." >&2
    exit 1
  fi
  echo ""
  echo "Volume removed. Start the stack again:"
  echo "  pnpm dev:aspire"
  echo ""
  echo "The API applies pending migrations automatically on startup in Development."
else
  echo "Resetting manual-path Postgres (docker compose, port 5433)..."
  docker compose down -v
  docker compose up -d
  wait_for_postgres

  echo "Applying migrations..."
  dotnet tool restore
  dotnet ef database update \
    --project apps/api/src/LitePress.Infrastructure \
    --startup-project apps/api/src/LitePress.WebApi

  echo ""
  echo "Database recreated. Connection string:"
  echo "  Host=localhost;Port=5433;Database=litepress;Username=litepress;Password=litepress"
fi
