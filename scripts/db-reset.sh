#!/usr/bin/env bash
# Drop the local PostgreSQL data volume and re-apply Marten storage schema.
#
# Usage:
#   bash scripts/db-reset.sh              # manual path (docker compose, port 5432)
#   bash scripts/db-reset.sh --aspire     # Aspire path (litepress-postgres-data volume)
set -euo pipefail

ROOT="$(cd "$(dirname "$0")/.." && pwd)"
cd "$ROOT"

ASPIRE=false
if [[ "${1:-}" == "--aspire" ]]; then
  ASPIRE=true
fi

wait_for_postgres() {
  for _ in $(seq 1 30); do
    if docker compose ps postgres 2>/dev/null | grep -q "(healthy)"; then
      return 0
    fi
    sleep 2
  done
  echo "error: PostgreSQL did not become healthy in time." >&2
  exit 1
}

if $ASPIRE; then
  echo "Stopping Aspire resources..."
  bash scripts/dev-aspire-stop.sh

  echo "Resetting Aspire PostgreSQL volume (litepress-postgres-data)..."
  if ! docker volume rm litepress-postgres-data -f 2>&1; then
    echo "warning: could not remove volume. Stop pnpm dev:aspire and retry." >&2
    exit 1
  fi
  echo ""
  echo "Volume removed. Start the stack again:"
  echo "  pnpm dev:aspire"
  echo ""
  echo "The API applies Marten schema automatically on startup in Development."
else
  echo "Resetting manual-path PostgreSQL (docker compose, port 5432)..."
  docker compose down -v
  docker compose up -d
  wait_for_postgres

  echo "Applying schema..."
  export ASPNETCORE_ENVIRONMENT=Development
  dotnet run --project apps/api/src/LitePress.WebApi -- --apply-schema-only

  echo ""
  echo "Database recreated. Connection string:"
  echo "  Host=127.0.0.1;Port=5432;Database=litepress;Username=litepress;Password=litepress"
fi
