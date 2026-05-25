#!/usr/bin/env bash
# Local E2E stack mirroring .github/workflows/e2e.yml
set -euo pipefail

ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
cd "$ROOT"

export ConnectionStrings__Database="Host=localhost;Port=5433;Database=litepress;Username=litepress;Password=litepress"
export JwtSettings__Secret="dev-secret-key-must-be-at-least-32-characters-long!"
export E2E_API_URL="http://localhost:5000"
export API_JWT_SECRET="dev-secret-key-must-be-at-least-32-characters-long!"
export PLAYWRIGHT_BASE_URL="http://localhost:3000"
export API_URL="http://localhost:5000"
export SITE_URL="http://localhost:3000"

echo "Starting Postgres..."
docker compose up -d

echo "Building API..."
cd apps/api
dotnet restore LitePress.slnx
dotnet build LitePress.slnx --configuration Release --no-restore
cd "$ROOT"

echo "Applying migrations..."
dotnet tool restore
dotnet ef database update \
  --project apps/api/src/LitePress.Infrastructure \
  --startup-project apps/api/src/LitePress.WebApi \
  --configuration Release

echo "Starting API in background..."
dotnet run --project apps/api/src/LitePress.WebApi --configuration Release --no-build --urls http://localhost:5000 &
API_PID=$!
trap 'kill $API_PID 2>/dev/null || true' EXIT

for i in $(seq 1 30); do
  if curl -sf http://localhost:5000/openapi/v1.json >/dev/null; then
    break
  fi
  if [ "$i" -eq 30 ]; then
    echo "API did not become ready in time."
    exit 1
  fi
  sleep 2
done

echo "Building web app..."
pnpm turbo build --filter=web

cd apps/web
pnpm exec playwright install chromium
pnpm exec playwright test
