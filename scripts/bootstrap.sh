#!/usr/bin/env bash
set -euo pipefail

ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
cd "$ROOT"

check_cmd() {
  if ! command -v "$1" >/dev/null 2>&1; then
    echo "warning: $1 is not on PATH. Install it before running LitePress." >&2
  fi
}

echo "Initializing git submodules..."
git submodule update --init --recursive

if pushd standards >/dev/null; then
  if tag=$(git describe --tags HEAD 2>/dev/null); then
    echo "Standards pinned to: $tag"
  else
    echo "warning: standards submodule has no tags yet." >&2
  fi
  popd >/dev/null
fi

echo "Checking prerequisites..."
check_cmd dotnet
check_cmd node
check_cmd pnpm
check_cmd docker

echo "Restoring pinned dotnet tools..."
dotnet tool restore

echo "Installing frontend dependencies..."
pnpm install

copy_example_if_missing() {
  local example="$1"
  local target="$2"
  if [[ ! -f "$target" && -f "$example" ]]; then
    cp "$example" "$target"
    echo "Created $target from example."
  fi
}

app_host="$ROOT/apps/api/src/LitePress.AppHost"
copy_example_if_missing "$app_host/Properties/launchSettings.json.example" "$app_host/Properties/launchSettings.json"
copy_example_if_missing "$app_host/appsettings.Development.json.example" "$app_host/appsettings.Development.json"
copy_example_if_missing "$ROOT/apps/admin/.env.example" "$ROOT/apps/admin/.env.local"

check_frontend_ui_scaffold() {
  local app_path="$1"
  local app_name="$2"
  local missing=""
  [[ -f "$app_path/postcss.config.mjs" ]] || missing+=" postcss.config.mjs"
  [[ -f "$app_path/components.json" ]] || missing+=" components.json"
  [[ -d "$app_path/components/ui" ]] || missing+=" components/ui/"
  if [[ ! -f "$app_path/app/globals.css" ]] || ! grep -q '@import "tailwindcss"' "$app_path/app/globals.css" 2>/dev/null; then
    missing+=" app/globals.css (@import tailwindcss)"
  fi
  if [[ -n "$missing" ]]; then
    echo "warning: $app_name missing UI scaffold:$missing. See docs/technical/development.md" >&2
  fi
}

check_frontend_ui_scaffold "$ROOT/apps/web" "apps/web"
check_frontend_ui_scaffold "$ROOT/apps/admin" "apps/admin"

echo ""
echo "Bootstrap complete."
echo "  Recommended: pnpm dev:aspire"
echo "  API docs:     {api-url}/scalar/v1 (Development)"
echo "  Admin OAuth:  cp apps/admin/.env.example apps/admin/.env.local"
echo "  Manual path:  bash scripts/dev-manual.sh"
