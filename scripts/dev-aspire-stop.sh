#!/usr/bin/env bash
# Stop LitePress Aspire resources left running after Ctrl+C.
#
# Usage: bash scripts/dev-aspire-stop.sh
set -euo pipefail

ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"

stop_matching() {
  local pattern="$1"
  local pids
  pids=$(pgrep -f "$pattern" 2>/dev/null || true)
  if [[ -z "$pids" ]]; then
    return 0
  fi
  echo "  Stopping processes matching: $pattern"
  # shellcheck disable=SC2086
  kill -TERM $pids 2>/dev/null || true
  sleep 1
  # shellcheck disable=SC2086
  kill -KILL $pids 2>/dev/null || true
}

echo "Stopping LitePress Aspire processes..."
stop_matching "LitePress.AppHost"
stop_matching "LitePress.WebApi"
stop_matching "$ROOT/apps/web"
stop_matching "$ROOT/apps/admin"

echo "Stopping Aspire PostgreSQL containers (litepress-postgres-data)..."
mapfile -t container_ids < <(docker ps -aq --filter "volume=litepress-postgres-data" 2>/dev/null || true)
if [[ ${#container_ids[@]} -eq 0 ]]; then
  echo "  No running containers using litepress-postgres-data."
else
  for id in "${container_ids[@]}"; do
    name="$(docker inspect --format '{{.Name}}' "$id" 2>/dev/null || echo "$id")"
    echo "  Removing container ${name} (${id})..."
    docker rm -f "$id" >/dev/null 2>&1 || true
  done
fi

echo ""
echo "Aspire stack stopped."
echo "  Start again: pnpm dev:aspire"
echo "  Reset DB:     pnpm db:reset:aspire"
