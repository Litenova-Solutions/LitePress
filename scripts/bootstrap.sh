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

echo ""
echo "Bootstrap complete."
echo "  Recommended: pnpm dev:aspire"
echo "  Admin OAuth:  cp apps/admin/.env.example apps/admin/.env.local"
echo "  Manual path:  bash scripts/dev-manual.sh"
